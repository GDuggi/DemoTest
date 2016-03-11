package aff.confirm.common.util;

/**
 * @author rnell
 * @date 2015-07-01
 */
public class RetrySpec {
    long retryInterval;
    int retryCount;

    public RetrySpec(long retryInterval, int retryCount) {
        this.retryInterval = retryInterval;
        this.retryCount = retryCount;
    }

}
