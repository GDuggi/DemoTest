package aff.confirm.common.util;

import java.io.Serializable;
import java.util.*;

/**
 * SempraBitSet converts sempra existing
 * integer masks( Hourly Mask, Minute Mask)
 * to BitSet and vise versa.
 *
 * Mask schema:
 *      1) Hourly Mask
 *         "00" is for bits of 30 & 31,
 *         bits 0-23 represent the daily 24 hours.
 *
 *      2) Minute Mask
 *         "01" is for bits 31 & 30,
 *         startMinute is placed within bits 14-29,
 *         durationMinute is placed within bits 0-13.
 *
 * SempraBitSet uses delegation to implement all BitSet public methods,
 * but only redefine public int size(), which defines
 * the immutable bit range.
 *
 * The size of SempraBitSet implies the Minutes interval each bit represent.
 * That is
 *     (Size of SempraBitSet) * (Minutes interval each bit represent) == 1440
 *
 */
public class SempraBitSet implements Cloneable, Serializable {

    public static final int HOURS_IN_DAY = 24;
    public static final int MINUTES_IN_HOUR = 60;
    public static final int MINUTES_IN_DAY = 1440;

    public static final int ALL_HOUR_MASK = (1 << 24) - 1;
    public static final Integer ALL_HOUR_INTEGER = new Integer(ALL_HOUR_MASK);

    public static final SempraBitSet DAILY_BIT_SET = createBitSetOn(1, 0, 1);

    public SempraBitSet(int size) {
        this.size = size;
        impl = new BitSet(size);
    }

    /**
     * Convenience method returns hourly SempraBitSet
     * with all bits are set.
     */
    public static SempraBitSet getAllHours() {
        SempraBitSet bitSet = new SempraBitSet(HOURS_IN_DAY);
        for (int i = 0; i < HOURS_IN_DAY; ++i) bitSet.set(i);
        return bitSet;
    }

    /**
     * Convenience method returns hourly SempraBitSet
     * with no bit is set.
     */
    public static SempraBitSet getNoHours() {
        return new SempraBitSet(HOURS_IN_DAY);
    }

    /**
     * Calculates the offpeak hourly mask given
     * the hourly mask.
     */
    public static int getOffPeakHourMask(int hourMask) {
        int mask = ~hourMask;
        return mask & ((1 << 24) - 1);
    }

    /**
     * Calculates the SempraBitSet of offpeak minute mask given
     * the peak minute mask.
     */
    public static SempraBitSet getOffPeakMinuteMask(int minuteMask) {

        int startMinute = getStartMinute(minuteMask);
        int duration = getDuration(minuteMask);
        int endMinute = startMinute + duration;
        int interval = getMaxInterval(minuteMask);
        int offPeakMask;
        SempraBitSet retval = new SempraBitSet(MINUTES_IN_DAY / interval);
        if (startMinute > 0) {
            offPeakMask = createMinuteMask(0, startMinute);
            retval.or(maskToBitSet(offPeakMask, interval));
        }

        if (endMinute < MINUTES_IN_DAY) {
            offPeakMask = createMinuteMask(endMinute, MINUTES_IN_DAY - endMinute);
            retval.or(maskToBitSet(offPeakMask, interval));
        }

        return retval;
    }

    /**
     * Creates a SempraBitSet given size, startBit, bitsToSet.
     * providing that size >= (startBit + bitsToSet)
     */
    public static SempraBitSet createBitSetOn(int size, int startBit, int bitsToSet) {
        SempraBitSet ret = new SempraBitSet(size);
        for (int i = 0; i < bitsToSet; ++i) {
            ret.set(startBit++);
        }
        return ret;
    }

    /**
     * Convenience method for
     *   createBitSetOn(int size, int startBit, int bitsToSet)
     */
    public static SempraBitSet createBitSetOn(int size) {
        return createBitSetOn(size, 0, size);
    }

    /**
     * Create a SempraBitSet given a mask & a interval.
     * @param mask mask to be converted
     * @param interval interval in minutes, which implies the
     *                 size of the SempraBitSet to be created.
     * @return SempraBitSet the BitSet representation of mask
     */
    public static SempraBitSet maskToBitSet(int mask, int interval){
        if (mask == 0) mask = ALL_HOUR_MASK;
        if (isHourlyMask(mask)) {
            if (interval == MINUTES_IN_HOUR)
                return hourMaskToBitSet(mask);
            else
                return hourMaskToBitSet(mask, interval);
        } else
            return minuteMaskToBitSet(mask, interval);
    }

    public static SempraBitSet maskToBitSet(int mask) {
        return maskToBitSet(mask, MINUTES_IN_HOUR);
    }

    /**
     * Calculates the minute mask
     * given the startMinute and duration.
     *
     * @param startMinute time in minutes
     * @param duration time in minutes
     */
    public static int createMinuteMask(int startMinute, int duration) {
        int min = (startMinute & ((1 << 17) - 1)) << 13;
        int dur = duration & ((1 << 13) - 1);
        return ((1 << 30) | min | dur);
    }

    /**
     * Calculates the hour mask given startTime and duration.
     * The result should always equals to
     *    (1 << (startTime + duration)) - (1 << startTime);
     *
     * @param startTime starting time in minutes
     * @param duration duration time in minutes
     */
    public static int createHourMask(int startTime, int duration) {
        startTime /= MINUTES_IN_HOUR;
        duration /= MINUTES_IN_HOUR;
        //return (1 << (startTime + duration)) - (1 << startTime);

        int durationMask = (1 << duration) - 1;
        durationMask <<= startTime;

        return durationMask;

    }

    /**
     * find the overlap of two possible different size SempraBitSets.
     */
    public static SempraBitSet and2(SempraBitSet bitSet1, SempraBitSet bitSet2) {

        if (bitSet1.length() == 0)
            return bitSet1;

        SempraBitSet[] bitSets = new SempraBitSet[]{bitSet1, bitSet2};
        toCompaitibleSets(bitSets);
        SempraBitSet newBitSet1 = bitSets[0], newBitSet2 = bitSets[1];
        newBitSet1.and(newBitSet2);
        return newBitSet1;

    }

    /**
     * Clears all of the bits in this bitSet1 whose corresponding bit is set in the specified bitSet2.
     * The two SempraBitSets may have different size
     */
    public static SempraBitSet andNot2(SempraBitSet bitSet1, SempraBitSet bitSet2) {

        if (bitSet1.length() == 0)
            return bitSet1;

        SempraBitSet[] bitSets = new SempraBitSet[]{bitSet1, bitSet2};
        SempraBitSet newBitSet1 = null, newBitSet2 = null;
        if (toCompaitibleSets(bitSets)) {
            newBitSet1 = (SempraBitSet)bitSets[1].clone();
            newBitSet2 = bitSets[0];
        }
        else {
            newBitSet1 = bitSets[0];
            newBitSet2 = bitSets[1];
        }

        newBitSet1.andNot(newBitSet2);
        return newBitSet1;

    }

    /**
     * find the exclusive or (xor) of two possible different size SempraBitSets.
     */
    public static SempraBitSet xor2(SempraBitSet bitSet1, SempraBitSet bitSet2) {

        SempraBitSet[] bitSets = new SempraBitSet[]{bitSet1, bitSet2};
        SempraBitSet newBitSet1 = null, newBitSet2 = null;
        if (toCompaitibleSets(bitSets)) {
            newBitSet1 = (SempraBitSet)bitSets[1].clone();
            newBitSet2 = bitSets[0];
        }
        else {
            newBitSet1 = bitSets[0];
            newBitSet2 = bitSets[1];
        }

        newBitSet1.xor(newBitSet2);
        return newBitSet1;

    }

    /**
     * Given two possible different resolution SempraBitSets
     * Convert them to compatible SempraBitSets
     * then reset the compatible SempraBitSets in the
     * Array passed in.
     * @return indicate the bitSets have been swaped or not.
     * Compatible SempraBitSets have the same resolution, i.e. same size.
     *
     * pre-condition: bitSets.length == 2
     */
    public static boolean toCompaitibleSets(SempraBitSet[] bitSets) {
        SempraBitSet bitSet1 = bitSets[0];
        SempraBitSet bitSet2 = bitSets[1];
        int size1 = bitSet1.size(), size2 = bitSet2.size();
        if (size1 == size2) {
            bitSets[0] = (SempraBitSet) bitSets[0].clone();
            return false;
        }

        int newDuration;
        boolean ret = false;
        SempraBitSet newBitSet1, newBitSet2;
        if ((size1 % size2) == 0) {
            newDuration = MINUTES_IN_DAY / size1;
            newBitSet1 = convertToDifferentDuration(bitSet2, newDuration);
            newBitSet2 = bitSet1;
            ret = true;
        } else if ((size2 % size1) == 0) {
            newDuration = MINUTES_IN_DAY / size2;
            newBitSet1 = convertToDifferentDuration(bitSet1, newDuration);
            newBitSet2 = bitSet2;
        } else {
            int size = size1 * size2;
            newDuration = MINUTES_IN_DAY / size;
            if ((MINUTES_IN_DAY % size) != 0)
                newDuration = 1;

            newBitSet1 = convertToDifferentDuration(bitSet1, newDuration);
            newBitSet2 = convertToDifferentDuration(bitSet2, newDuration);
        }
        bitSets[0] = newBitSet1;
        bitSets[1] = newBitSet2;

        return ret;
    }

    /**
     * convert a given mask to a particular DailyDuration
     * @param originalBitSet
     * @param newDuration
     */
    public static SempraBitSet convertToDifferentDuration(SempraBitSet originalBitSet, int newDuration) {
        SempraBitSet newBitSet = new SempraBitSet(MINUTES_IN_DAY / newDuration);
        int scale = 0;
        if (originalBitSet.size() > newBitSet.size()) {//creating a smaller bitset
            scale = originalBitSet.size() / newBitSet.size();
            convertToSmallerBitSet(originalBitSet, newBitSet, scale);
        } else {
            scale = newBitSet.size() / originalBitSet.size();
            convertToLargetBitSet(originalBitSet, newBitSet, scale);
        }
        return newBitSet;
    }

    private static void convertToLargetBitSet(SempraBitSet originalBitSet, SempraBitSet newBitSet, int scale) {
        int index = 0;
        for (int i = 0; i < originalBitSet.size(); i++) {
            if (originalBitSet.get(i)) {
                for (int j = index; j < index + scale; j++) {
                    newBitSet.set(j);
                }
            }
            index += scale;
        }
    }

    private static void convertToSmallerBitSet(SempraBitSet originalBitSet, SempraBitSet newBitSet, int scale) {
        int index = 0;

        for (int i = 0; i < originalBitSet.size(); i += scale) {
            boolean isOn = originalBitSet.get(i) || originalBitSet.get(i + 1);
            if (isOn)
                newBitSet.set(index);
            index++;
        }
    }

    /**
     * When convert SempraBitSet from a smaller duration to a larger duration of minutes in interval,
     * the original bit set may not have all the bits set confirmed to new duration boundary,
     * so we may need to break the bit set to a list of SempraBitSet by the set bit counts with
     * the factor boundary.
     * <pre>
     * For example: if we have 15 min mask as
     * H1    H2    H3    H4    H5    H6
     * 1111, 1110, 1100, 1000, 1111, 1010
     * converting to 60 min will have a list of SempraBitSet as
     * {H4, H3H6, H2, H1H5}
     * and the index into the returning list plus one
     * is the number of bits set in the original SempraBitSet within the factor boundary.
     * </pre>
     *
     * @return a list of SempraBitSet which index plus one corresponse quantity multiple factor
     * if the SempraBitSet is not null.
     */
    public static ArrayList toLargerDuration(SempraBitSet theBitSet, int oldDuration, int newDuration) {
        ArrayList newBitSetList = new ArrayList();
        int factor = newDuration / oldDuration;
        for (int k=0; k<factor; k++) {
             newBitSetList.add(null);
        }

        int size = theBitSet.size();
        for (int i=0; i<size; i += factor) {
            int count = 0;
            for (int j=0; j<factor; j++) {
                if (theBitSet.get(i + j))
                    count++;
            }
            if (count > 0) {
                SempraBitSet countedBitSet = (SempraBitSet) newBitSetList.get(count - 1);
                if (countedBitSet == null) {
                    countedBitSet = new SempraBitSet(MINUTES_IN_DAY / newDuration);
                    newBitSetList.set(count - 1, countedBitSet);
                }
                countedBitSet.set(i / factor);
            }
        }
        return newBitSetList;
    }

    public void shiftBitSet(int hourOffset) {
        int offset = hourOffset * size / HOURS_IN_DAY;
        if (offset > 0) {
            for (int i=size() - 1 - offset; i>=0; i--) {
                if (get(i))
                    set(i+offset);
                else
                    clear(i+offset);
            }
            for (int i=0; i<offset; i++)
                clear(i);
        }
        else if (offset < 0) {
            for (int i= -offset; i<size(); i++) {
                if (get(i))
                    set(i+offset);
                else
                    clear(i+offset);
            }
            for (int i=size() - 1; i>= size() + offset; i--)
                clear(i);
        }
    }

    /**
     * Tests to see if this SempraBitSet includes the specified SempraBitSet.
     * Include means all the bits are set in the specified SempraBitSet
     * must be set in the current SempraBitSet.
     */
    public boolean isInclusive(SempraBitSet other) {
        SempraBitSet[] bitSets = new SempraBitSet[]{this, other};
        SempraBitSet bitSet1, bitSet2;
        if (toCompaitibleSets(bitSets)) {
            bitSet1 = bitSets[1];
            bitSet2 = bitSets[0];
        } else {
            bitSet1 = bitSets[0];
            bitSet2 = bitSets[1];
        }
        int sz = bitSet1.size();
        for (int i = 0; i < sz; i++) {
            if (bitSet2.get(i) && !bitSet1.get(i))
                return false;
        }
        return true;
    }

    /**
     * Tests to see if this SempraBitSet and the specified SempraBitSet
     * are exclusive each other.
     */
    public boolean isExclusive(SempraBitSet other) {
        SempraBitSet[] bitSets = new SempraBitSet[]{this, other};
        toCompaitibleSets(bitSets);

        SempraBitSet bitSet1 = bitSets[0];
        SempraBitSet bitSet2 = bitSets[1];

        int sz = bitSet1.size();
        for (int i = 0; i < sz; i++) {
            if (bitSet1.get(i) && bitSet2.get(i))
                return false;
        }
        return true;
    }

    /**
     * Calculate the exclusive part SempraBitSet for the specified SempraBitSet
     * from this current SempraBitSet.
     */
    public SempraBitSet getExclusive(SempraBitSet other) {
        SempraBitSet[] bitSets = new SempraBitSet[]{this, other};
        SempraBitSet bitSet1, bitSet2;
        if (toCompaitibleSets(bitSets)) {
            bitSet1 = bitSets[1];
            bitSet2 = bitSets[0];
        } else {
            bitSet1 = bitSets[0];
            bitSet2 = bitSets[1];
        }
        int sz = bitSet1.size();
        SempraBitSet newBitSet = new SempraBitSet(sz);
        for (int i = 0; i < sz; i++) {
            if (bitSet2.get(i) && !bitSet1.get(i))
                newBitSet.set(i);
        }

        return newBitSet;
    }

    /**
     * Calculate the inclusive part SempraBitSet for the specified SempraBitSet
     * within this current SempraBitSet.
     */
    public SempraBitSet getInclusive(SempraBitSet other) {
        SempraBitSet[] bitSets = new SempraBitSet[]{this, other};
        SempraBitSet bitSet1, bitSet2;
        if (toCompaitibleSets(bitSets)) {
            bitSet1 = bitSets[1];
            bitSet2 = bitSets[0];
        } else {
            bitSet1 = bitSets[0];
            bitSet2 = bitSets[1];
        }
        int sz = bitSet1.size();
        SempraBitSet newBitSet = new SempraBitSet(sz);
        for (int i = 0; i < sz; i++) {
            if (bitSet2.get(i) && bitSet1.get(i))
                newBitSet.set(i);
        }

        return newBitSet;
    }

    /**
     * returns the time interval in minutes each bitset represents.
     */
    public int getInterval() {
        return MINUTES_IN_DAY / size();
    }

    public boolean isContiguous() {
        boolean foundGap = false;
        boolean foundStartBit = false;
        for (int i = 0; i < size(); i++) {
            boolean isOn = impl.get(i);
            if (isOn) {
                if (foundGap)
                    return false;

                if (!foundStartBit)
                    foundStartBit = true;
            } else if (foundStartBit)
                foundGap = true;

        }

        return true;
    }

    public boolean isSingleMask() {
        if (size <= 24) return true;

        int factor = MINUTES_IN_HOUR / getInterval();
        int startIndex = -1;
        int endIndex = -1;
        for (int i = 0; i < size; i++) {
             if (get(i)) {
                 if (startIndex == -1)
                     startIndex = i;

                 if (endIndex != -1)
                     return false;

             } else {
                 if (startIndex != -1) {
                     if (endIndex == -1)
                         endIndex = i;
                     else if ((startIndex % factor) == 0 && (endIndex % factor) == 0)
                         startIndex = endIndex = -1;
                 }
             }
        }
        return true;
    }

   /**
    * build a list of SempraBitSet which can be converted
    * to a single mask.
    * 1) first element will be houly bitSet if any.
    * 2) then followed by contiguous minute bitSet.
    */
    public List buildSingleMaskBitSet() {
        List list = new ArrayList();
        if (size <= 24) {
            list.add(this);
            return list;
        }

        SempraBitSet hourlyBitSet = new SempraBitSet(size);
        int factor = MINUTES_IN_HOUR / getInterval();
        int startIndex = -1;
        int i = -1;
        for (i = 0; i < size; i++) {
             if (get(i)) {
                 if (startIndex == -1)
                     startIndex = i;
                 else if (((i+1) % factor) == 0) { // at hourly boundary
                     if ((i + 1 - startIndex) >= factor) {
                         //set hourly bitset
                         hourlyBitSet.set(i + 1 - factor, factor);
                         //create minute bitset
                         if (startIndex <= (i - factor))
                             list.add(createBitSetOn(size, startIndex, i + 1 - startIndex - factor));

                         startIndex = -1;
                     }
                 }
             } else {
                 if (startIndex != -1) {
                     list.add(createBitSetOn(size, startIndex, i - startIndex));
                     startIndex = -1;
                 }
             }
        }
        if (startIndex != -1)
            list.add(createBitSetOn(size, startIndex, i - startIndex));

        // add the hourly bitset at the front.
        if (!hourlyBitSet.isEmpty())
            list.add(0, hourlyBitSet);

        return list;
    }

   /**
    * build a list of SempraBitSet which can be converted
    * to a single mask.
    * 1) first element will be houly bitSet if any.
    * 2) then followed by contiguous minute bitSet.
    */
    public List buildSingleMaskBitSet2() {
        if (size <= 24) {
            List list = new ArrayList();
            list.add(this);
            return list;
        }

        SempraBitSet hourlyBitSet = extractHourlyBitSet();
        if (hourlyBitSet.isEmpty())
            return buildSingleMinuteBitSet();


        SempraBitSet clonedBitSet = (SempraBitSet) clone();
        clonedBitSet.andNot(hourlyBitSet);
        List list = clonedBitSet.buildSingleMinuteBitSet();
        list.add(0, hourlyBitSet);

        return list;
    }

    public SempraBitSet extractHourlyBitSet() {
        SempraBitSet hourlyBitSet = new SempraBitSet(size);
        int factor = MINUTES_IN_HOUR / getInterval();
        for (int i = 0; i < size; i++) {
            if (get(i)) {
                if ((i % factor) != 0) continue;
                boolean hourlySet = true;
                for (int j = 1; j < factor; j++) {
                    if (!get(i + j)) {
                        hourlySet = false;
                        break;
                    }
                }
                i += factor - 1;
                if (hourlySet)
                    hourlyBitSet.set(i + 1 - factor, factor);

            }
        }
        return hourlyBitSet;
    }

    private List buildSingleMinuteBitSet() {
        List list = new ArrayList();
        int startIndex = -1;
        int i = -1;
        for (i = 0; i < size; i++) {
             if (get(i)) {
                 if (startIndex == -1)
                     startIndex = i;
             } else {
                 if (startIndex != -1) {
                     list.add(createBitSetOn(size, startIndex, i - startIndex));
                     startIndex = -1;
                 }
             }
        }

        if (startIndex != -1)
            list.add(createBitSetOn(size, startIndex, i - startIndex));

        return list;
    }

    /**
     * Is XOR operation of the specified SempraBitSet and this SempraBitSet
     * formed a Contiguous minutes mask.
     */
    public boolean isContiguous(SempraBitSet aBitSet) {
        if (size() != aBitSet.size())
            return false;

        int startMin = getFirstMinuteSet() < aBitSet.getFirstMinuteSet() ? getFirstMinuteSet() :  aBitSet.getFirstMinuteSet();
        int endMin = getLastMinuteSet() > aBitSet.getLastMinuteSet() ? getLastMinuteSet() : aBitSet.getLastMinuteSet();
        int startIndex = startMin * size() / MINUTES_IN_DAY;
        int endIndex = endMin * size() / MINUTES_IN_DAY;

        for (int i=startIndex; i<endIndex; i++) {
             if (!(get(i) ^ aBitSet.get(i)))
                 return false;
        }

        return true;
    }

    public static boolean isContiguous(SempraBitSet bitSet1, SempraBitSet bitSet2) {
        SempraBitSet[] bitSets = new SempraBitSet[]{bitSet1, bitSet2};
        toCompaitibleSets(bitSets);
        SempraBitSet newBitSet1 = bitSets[0], newBitSet2 = bitSets[1];
        return newBitSet1.isContiguous(newBitSet2);
    }

    /**
     * Can this SempraBitSet be represented in hourly mask (not minute mask)
     */
    public boolean isHourlyMask() {
        if (size <= 24) return true;
        int factor = MINUTES_IN_HOUR / getInterval();
        for (int i = 0; i < size; i++) {
            if (get(i)) {
                if ((i % factor) != 0) return false;
                for (int j = 1; j < factor; j++)
                    if (!get(i + j)) return false;
                i += factor - 1;
            }
        }

        return true;
    }

    public List toMask() {
        return toMask(true);
    }

    /**
     * Converts to a list of hour mask or
     * a list of minute mask. If biasHourly is true
     * and this SempraBitSet can be
     * converted to hour mask, there is
     * only one element in the list.
     *
     * If bitSet is hour BitSet mask and biasHourly is true,
     * only one element of hour mask will be contained
     * in the returned list.
     * Otherwise, each mask in the list has continuous duration.
     *
     * This method will try to convert to
     * hour mask if biasHourly is passed in
     * as true.
     *
     */
    public List toMask(boolean biasHourly) {
        boolean mustBeMinutes = false, hasAnyMinutesOnly = false;
        int minutesInInterval = MINUTES_IN_DAY / size;

        // if(minutesInInterval == MINUTES_IN_HOUR)
        //   biasHourly = true;

        List minuteList = new ArrayList();
        int hoursMask = 0, startTime = -1, duration = 0;

        for (int i = 0; i < impl.length(); i++) {
            if (impl.get(i)) {
                if (startTime == -1) {
                    //startTime = (i + 1) *  minutesInInterval;
                    startTime = i * minutesInInterval;
                    duration = minutesInInterval;
                } else {
                    duration += minutesInInterval;
                }
            }

            if ((i == impl.length() - 1) || (!impl.get(i)) && (startTime != -1)) {
                mustBeMinutes = ((startTime % MINUTES_IN_HOUR) != 0) || ((duration % MINUTES_IN_HOUR) != 0);
                if (mustBeMinutes)
                    hasAnyMinutesOnly = true;

                minuteList.add(new Integer(createMinuteMask(startTime, duration)));

                if (!mustBeMinutes)
                    hoursMask |= createHourMask(startTime, duration);

                startTime = -1;
            }
        } // for

        if (biasHourly && !hasAnyMinutesOnly) {
            List hourList = new ArrayList();
            hourList.add(new Integer(hoursMask));
            return hourList;
        } else
            return minuteList;

    }

    public boolean isEmpty() {
        return length() == 0;
    }

    // BisSet interface
    /**
     * The immutable size of SempraBitSet which
     * implies the Minutes interval each bit represent.
     * That is
     *     (Size of SempraBitSet) * (Minutes interval each bit represent) == 1440
     */
    public int size() {
        return size;
    }

    public int length() {
        return impl.length();
    }

    public boolean get(int index) {
        if (index >= size) {
            throw new IndexOutOfBoundsException(Integer.toString(index));
        }
        return impl.get(index);
    }

    public void set(int index) {
        if (index >= size) {
            throw new IndexOutOfBoundsException(Integer.toString(index));
        }
        impl.set(index);
    }

    public void set(int startIndex, int bitsToSet) {
        if (startIndex + bitsToSet > size) {
            throw new IndexOutOfBoundsException(Integer.toString(startIndex + bitsToSet));
        }
        for (int i=startIndex; i<(startIndex + bitsToSet); i++)
            impl.set(i);
    }

    public void clear(int index) {
        if (index >= size) {
            throw new IndexOutOfBoundsException(Integer.toString(index));
        }
        impl.clear(index);
    }

    public void clear() {
        for (int i=0; i<size; i++)
            impl.clear(i);
    }

    public int hashCode() {
        return impl.hashCode();
    }

    public boolean equalsWithoutConvert(Object obj) {
        if (obj == null || !(obj instanceof SempraBitSet)) return false;

        SempraBitSet s = (SempraBitSet) obj;
        if (s.size != this.size) {
            return false;
        }

        return impl.equals(s.impl);
    }

    public boolean equals(Object obj) {
        if (obj == null || !(obj instanceof SempraBitSet)) return false;

        SempraBitSet s = (SempraBitSet) obj;
        if (s.size != this.size) {
            return equals(this, s);
        }

        return impl.equals(s.impl);
    }

    static boolean equals(SempraBitSet bitSet1, SempraBitSet bitSet2) {
        SempraBitSet[] bitSets = new SempraBitSet[]{bitSet1, bitSet2};
        toCompaitibleSets(bitSets);
        return bitSets[0].equals(bitSets[1]);
    }

    public Object clone() {
        SempraBitSet result = null;
        try {
            result = (SempraBitSet) super.clone();
        } catch (CloneNotSupportedException e) {
            throw new Error();
        }
        result.impl = (BitSet) impl.clone();
        result.size = size;

        return result;
    }

    public BitSet toBitSet() {
        return (BitSet) impl.clone();
    }

    /**
     * Convert a mask to user friendly string representation.
     * 1) Hourly mask starts with "HE" - hour ending,
     *    for example in hr 1, HE1, 8 to 23 hour HE8-23
     * 2) Minute mask use h1:m1-h2:m2
     *    for example 8:15 to 8:30  will becomes 8:15-8:30
     * @param mask
     * @return
     */
    public static String toString(int mask) {
        if (isHourlyMask(mask)) {
            SempraBitSet newBitSet = SempraBitSet.maskToBitSet(mask);
            return newBitSet.toFormatString();
        } else {
            // If the min mask can be converted to hour mask, use hour representation.
            if (canConvertToHourMask(mask)) {
                SempraBitSet newBitSet = SempraBitSet.minuteMaskToBitSet(mask, MINUTES_IN_HOUR);
                return newBitSet.toFormatString();
            } else {
                int startMin = SempraBitSet.getStartMinute(mask);
                int endMin = startMin + SempraBitSet.getDuration(mask);
                return toHHMMString(startMin) + "-" + toHHMMString(endMin);
            }
        }
    }

    public static String toHHMMString(int minute) {
        int hr = minute / MINUTES_IN_HOUR;
        int min = minute % MINUTES_IN_HOUR;
        StringBuffer buf = new StringBuffer();
        if (hr < 10) buf.append("0");
        buf.append(hr);
        buf.append(":");
        if (min < 10) buf.append("0");
        buf.append(min);
        return buf.toString();
    }

    /**
     * Convert SempraBitSet to user friendly string representation.
     * 1) Hourly mask starts with "HE" - hour ending,
     *    for example in hr 1, HE1, 8 to 23 hour HE8-23
     * 2) Minute mask use h1:m1-h2:m2
     *    for example 8:15 to 8:30  will returns 8:15-8:30
     */
    public String toFormatString() {
        StringBuffer buf = new StringBuffer("HE");
        boolean first = true;
        boolean inProcess = false;
        int i = 0, size = size();
        boolean isHourly = size == HOURS_IN_DAY;
        for (i = 0; i < size; i++) {
            if (get(i)) {
                if (!inProcess) {
                    if (first)
                        first = false;
                    else
                        buf.append(",");

                    buf.append(fromIndexToHHMM(i, size, true));
                    inProcess = true;
                }
            } else {
                if (inProcess) {
                    inProcess = false;
                    // For hourly, display "-" only for more than 1 hour duration
                    if (!isHourly || (i > 1 && get(i-2))) {
                        buf.append("-");
                        buf.append(fromIndexToHHMM(i, size, false));
                    }
                }
            }
        }
        if (inProcess) {
            // For hourly, display "-" only for more than 1 hour duration
            if (!isHourly || (i > 1 && get(i-2))) {
                buf.append("-");
                buf.append(fromIndexToHHMM(i, size, false));
            }
        }
        return buf.toString();
    }

    private static String fromIndexToHHMM(int setIndex, int size, boolean begin) {
        if (size == HOURS_IN_DAY) {
            if (begin) setIndex++;
            return new Integer(setIndex).toString();
        }
        else {
            int scale = size / HOURS_IN_DAY;
            int hour = (setIndex / scale);
            String hr = "0";
            if (hour >= 10)
                hr = new Integer(hour).toString();
            else
                hr += hour;

            int minute = ((setIndex % scale) * (MINUTES_IN_HOUR / scale));
            String min = "0";
            if (minute >= 10)
                min = new Integer(minute).toString();
            else
                min += minute;

            return hr + ":" + min;
        }
    }

    public String toString() {
        String ret = "SempraBitSet@{size=" + size + "}[";
        for (int i = 0; i < size; ++i) {
            if (impl.get(i))
                ret += "1";
            else
                ret += "0";
            if (i != (size - 1))
                ret += ",";
        }
        ret += "]";
        return ret;
    }

    public String[] toStringArray() {
        String[] ret = new String[size];
        Arrays.fill(ret,"");
        for (int i = 0; i < size; ++i) {
            if (impl.get(i))
                ret[i] = "1";
            else
                ret[i] = "0";
        }
        return ret;
    }

    public void andNot(SempraBitSet set) {
        if (size != set.size)
            throw new RuntimeException("andNot(): not equal size [" + size + "," + set.size + "]");

        impl.andNot(set.impl);

    }

    public void and(SempraBitSet set) {
        if (size != set.size)
            throw new RuntimeException("and(): not equal size [" + size + "," + set.size + "]");

        impl.and(set.impl);
    }

    public void or(SempraBitSet set) {
        if (size != set.size)
            throw new RuntimeException("or(): not equal size [" + size + "," + set.size + "]");

        impl.or(set.impl);
    }


    public void xor(SempraBitSet set) {
        if (size != set.size)
            throw new RuntimeException("xor(): not equal size [" + size + "," + set.size + "]");

        impl.xor(set.impl);
    }
    // end of BitSet interface

    /**
     * Converts hourly mask to an SempraBitSet with size == 24
     * @param hourMask the hourly mask used to convert to a <code>SempraBitSet</code>
     * @return SempraBitSet The SempraBitSet created from the hourly mask
     *        to the desired SempraBitSet
     */
    private static SempraBitSet hourMaskToBitSet(int hourMask) {

        SempraBitSet bitSet = new SempraBitSet(HOURS_IN_DAY);
        int mask;
        for (int i = 0; i < bitSet.size(); ++i) {
            mask = 1 << i;
            if ((hourMask & mask) == mask) bitSet.set(i);
        }

        return bitSet;
    }

    /**
     * @param mask the mask used to convert to a <code>SempraBitSet</code>
     * @param interval used to calculate for a minutes mask
     * @return the SempraBitSet The SempraBitSet created from the minute mask
     */
    private static SempraBitSet minuteMaskToBitSet(int mask, int interval) {

        int duration = getDuration(mask);
        if ((duration % interval) != 0) throw new RuntimeException("Invalid duration=" + duration + " given [mask=" + mask + ",interval=" + interval + "]");

        int size = MINUTES_IN_DAY / interval;
        SempraBitSet bitSet = new SempraBitSet(size);

        duration /= interval;
        int startMinute = getStartMinute(mask);
        if ((startMinute % interval) != 0) throw new RuntimeException("Invalid startMinute=" + startMinute + " given [mask=" + mask + ",interval=" + interval + "]");

        startMinute /= interval;

        for (int i = startMinute; i < (startMinute + duration); i++) {
            bitSet.set(i);
        }

        return bitSet;
    }

    public static int getDuration(int mask) {
        return mask & ((1 << 13) - 1);
    }

    public static int getStartMinute(int mask) {
        return (mask & (((1 << 17) - 1) << 13)) >> 13;
    }

    static int[] possibleInterval = new int[]{60, 30, 15, 10, 5};

    /**
     *  find out the maximium Interval for a minuteMask.
     */
    public static int getMaxInterval(int mask) {
        if (isHourlyMask(mask)) return MINUTES_IN_HOUR;

        int startMinute = getStartMinute(mask);
        int duration = getDuration(mask);
        int minuteDuration;
        for (int i = 0; i < possibleInterval.length; i++) {
            minuteDuration = possibleInterval[i];
            if ((startMinute % minuteDuration) == 0 &&
                    (duration % minuteDuration) == 0) {
                return minuteDuration;
            }
        }
        return 1;
    }

    /**
     * Determine a given mask is hourly or minute.
     */
    public static boolean isHourlyMask(int mask) {
        if ((mask >> 31) != 0) throw new RuntimeException("Negative mask[" + mask + "] is invalid");
        return (((mask >> 30) & 1) == 0);
    }

    /**
     * Determine the given minute mask can be converted to hour mask or not
     * @param minMask
     * @return
     */
    public static boolean canConvertToHourMask(int minMask) {
        int startMin = getStartMinute(minMask);
        int duration = getDuration(minMask);
        return (startMin % MINUTES_IN_HOUR) == 0 && (duration % MINUTES_IN_HOUR) == 0;
    }

    /**
     * This method is a complement of SempraBitSet hourMaskToBitSet(int mask)
     * Converts hourly mask to an SempraBitSet with size != 24
     * @param hourMask the hourly mask used to convert to a <code>SempraBitSet</code>
     * @return SempraBitSet The SempraBitSet created from the hourly mask
     */
    private static SempraBitSet hourMaskToBitSet(int hourMask, int interval) {
        if (interval == MINUTES_IN_DAY && hourMask != 1)
            interval = MINUTES_IN_HOUR;

        int sz = MINUTES_IN_DAY / interval;
        SempraBitSet bitSet = new SempraBitSet(sz);
        int bitsPerInterval = MINUTES_IN_HOUR / interval;

        if (bitsPerInterval > 0) {
            for (int i = 0; i < HOURS_IN_DAY; i++) {
                int mask = 1 << i;
                if ((hourMask & mask) == mask) {
                    int startBit = i * bitsPerInterval;
                    for (int x = 0; x < bitsPerInterval; x++)
                        bitSet.set(startBit + x);
                }
            }
        } else {
            bitsPerInterval = interval / MINUTES_IN_HOUR;
            for (int j = 0; j < sz; j++) {
                for (int y = 0; y < bitsPerInterval; y++) {
                    int mask = 1 << (j + y);
                    if ((hourMask & mask) != mask)
                            throw new RuntimeException("Not a valid mask[" + hourMask + "] for the minute interval[" + interval + "]");
                }
                bitSet.set(j);
            }
        }
        return bitSet;
        /*
        int[] masks = hourMaskToMinuteMaskList(mask);
        SempraBitSet ret = null;
        for(int i=0; i<masks.length; i++) {
           if(ret == null)
              ret = minuteMaskToBitSet(masks[i], interval);
           else
              ret.or(minuteMaskToBitSet(masks[i], interval));
        }
        return ret;
        */
    }

    /**
     * converts the hourMask to a list of
     * minuteMask.
     *
     * @param hourMask the hourly mask
     * @return a integer array of converted minute mask.
     */
    private static int[] hourMaskToMinuteMaskList(int hourMask) {
        int index = 0;
        int[] result = new int[HOURS_IN_DAY];
        int startMinute = -1;
        int duration = 0;
        boolean setFlag = false;
        for (int i = 0; i < HOURS_IN_DAY; i++) {
            setFlag = (hourMask & (1 << i)) != 0;
            if (setFlag) {
                if (startMinute == -1) {
                    startMinute = i * MINUTES_IN_HOUR;
                    duration = MINUTES_IN_HOUR;
                } else {
                    duration += MINUTES_IN_HOUR;
                }
            }

            if ((i == HOURS_IN_DAY - 1) || (!setFlag && startMinute != -1)) {
                result[index++] = createMinuteMask(startMinute, duration);
                startMinute = -1;
            }
        }
        int[] ret = new int[index];
        for (int j = 0; j < index; j++)
            ret[j] = result[j];

        return ret;
    }

    public int getFirstIndexSet() {
        for (int i = 0; i < impl.size(); i++) {
            if (impl.get(i)) {
                //Found the First bit that is set
                return i;
            }
        }
        return -1;
    }

    public int getLastIndexSet() {
        for (int i = (impl.size() - 1); i >= 0; i--) {
            if (impl.get(i)) {
                //Found the Last bit that is set
                return i;
            }
        }
        return -1;
    }

    /**
     * returns the first minute set in the current SempraBitSet, returns a -1 if not minute is set...
     */
    public int getFirstMinuteSet() {
        for (int i = 0; i < impl.size(); i++) {
            if (impl.get(i)) {
                //Found the First bit that is set
                return i * getInterval();
            }
        }
        return -1;
    }

    /**
     * returns the last minute ( + 1) set in the current SempraBitSet, returns a -1 if not minute is set...
     * NOTE: it returns one extra minute... for example: if only hour 1 was set, this function will return 60 and not 59.
     */
    public int getLastMinuteSet() {
        for (int i = (impl.size() - 1); i >= 0; i--) {
            if (impl.get(i)) {
                //Found the First bit that is set
                return (i + 1) * getInterval();
            }
        }
        return -1;
    }

    public int getTotalMinuteCount() {
        return getInterval() * getCount();
    }

    /**
     * Find out how many bit are set
     */
    public int getCount() {
        int count = 0;
        for (int i = 0; i < impl.size(); i++) {
            if (impl.get(i))
                count++;
        }
        return count;
    }

    private int size;
    private java.util.BitSet impl;


   /**
    * Given a hour mask, convert the hour mask to a list of contiguous start hour end hour period
    */
    public static List convertHrsMaskToPeriodList(int hrsMask) {
       SempraBitSet bitSet = maskToBitSet(hrsMask);
       return converHrsBitSetToPeriodList(bitSet);
   }

   private static List converHrsBitSetToPeriodList(SempraBitSet bitSet) {
       int len = bitSet.length();
       boolean inPeriod = false;
       int start = 0, end = 0;
       ArrayList periodList = new ArrayList();
       for (int i=0; i<len; i++) {
            if (bitSet.get(i)) {
                if (!inPeriod) {
                    start = i;
                    inPeriod = true;
                }
                end = i;
            }
            else {
                if (inPeriod) {
                    inPeriod = false;
                    periodList.add(new int[]{start, end+1});
                }
            }
       }
       if (inPeriod)
           periodList.add(new int[]{start, end+1});

       return periodList;
    }

    public static void main(String[] args) {
        SempraBitSet bitSet = new SempraBitSet(24);


        for (int i=6; i<12; i++)
             bitSet.set(i);
        bitSet.set(0);
        bitSet.set(18);
        bitSet.set(23);
        System.out.println(bitSet);

        List periodList = converHrsBitSetToPeriodList(bitSet);
        Iterator it = periodList.iterator();
        while (it.hasNext()) {
            int[] period = (int[]) it.next();
            int startHour = period[0];
            int endHour = period[1];
            System.out.println(startHour + " - " + endHour);
        }


    }

}