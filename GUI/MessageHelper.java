package tc.util;
import java.util.concurrent.Future;
import org.apache.log4j.Logger;
import com.amphora.als.publisher.BusPositionPublisher;
import com.amphora.als.publisher.BusPositionPublisherSingletonBuilder;
import com.amphora.model.PositionDTO;
import com.rabbitmq.client.Address;
import com.tc.frameworks.ictseos.eoposition.Position;
public class MessageHelper {
	private static final Logger log = Logger.getLogger(MessageHelper.class);
	public static boolean publish(Position pos, String pSInd) {
		boolean retval = false;
    	log.info("Inside publish ********");
    	try {
			if (pos != null) {
		    	String rabbitMQServer = System.getProperty("amp.rabbitq.server","172.16.143.199");
		    	String rabbitMQUser = System.getProperty("amp.rabbitq.username","guest");
		    	String rabbitMQPassword = System.getProperty("amp.rabbitq.password","guest");
		    	String rabbitMQPort = System.getProperty("amp.rabbitq.port","5672");

		    	log.info("amp.rabbitq.server  ="+ rabbitMQServer );
		    	log.info("amp.rabbitq.port    ="+ rabbitMQPort );
		    	log.info("amp.rabbitq.username="+ rabbitMQUser );
		    	log.info("amp.rabbitq.password="+ rabbitMQPassword ); //TODO: to be commented when checkin

		    	System.out.println("amp.rabbitq.server  ="+ rabbitMQServer );
		    	System.out.println("amp.rabbitq.port    ="+ rabbitMQPort );
		    	System.out.println("amp.rabbitq.username="+ rabbitMQUser );
		    	System.out.println("amp.rabbitq.password="+ rabbitMQPassword ); //TODO: to be commented when checkin
		    	
		    	BusPositionPublisher provider = 
		    			BusPositionPublisherSingletonBuilder.reusableBuilder()
		    		.rabbitMQAddresses(new Address(rabbitMQServer, Integer.parseInt(rabbitMQPort)))
		    		.rabbitMQUser(rabbitMQUser)
		    		.rabbitMQPassword(rabbitMQPassword)
		    		.build();
		    	
				PositionDTO dto = new PositionDTO();
				
				dto.setPortId(pos.getRealPortNum().intValue());
				if (pos.getPosNum()!=null)
					dto.setPosNum(pos.getPosNum().intValue());
				if (pSInd.equals("P") ) {
					if (pos.getLongQty()!=null )
						dto.setQuantity(pos.getLongQty().floatValue());
				} else {
					//S
					if (pos.getShortQty()!=null )
						dto.setQuantity(pos.getShortQty().floatValue());
				}
				dto.setCommodityCode(pos.getCmdtyCode());
				dto.setMarketCode(pos.getMktCode());
				dto.setTradingPeriod(pos.getTradingPrd());
				dto.setFormulaName(pos.getFormulaName());
				if (pos.getFormulaNum()!=null)
					dto.setFormulaNum(pos.getFormulaNum().intValue());
				if (pos.getTransId()!=null)
					dto.setTransactionId(pos.getTransId().intValue());
				if (pos.getPricedQty()!=null)
					dto.setPriceQty(pos.getPricedQty().floatValue());
				if (pos.getPriceUom() != null)
					dto.setPriceQtyUom(pos.getPriceUom().getUomShortName());
				if (pos.getSecPricedQty() != null)
					dto.setSecPriceQty(pos.getSecPricedQty().floatValue());
				if (pos.getPriceUom() != null)
					dto.setSecPriceUom(pos.getPriceUom().getUomShortName());
				displayDTO(dto);
				Future<Boolean> future = provider.publishPosition(dto);
				retval = future.get();
				log.info("return from publishPosition(dto)="+ retval);
			}
    	} catch (Throwable e) {
    		e.printStackTrace();
    		log.error(e);
    	} finally {
		   log.info("DONE: Inside publish (retval="+retval+") ********");
		   System.out.println("DONE: Inside publish (retval="+retval+") ********");
    	}
		return retval;
	}
	
	private static void displayDTO(PositionDTO dto) {
		System.out.println("-----------start-----------");
		System.out.println("portId= " + dto.getPortId());
		System.out.println("posNum= " + dto.getPosNum());
		System.out.println("quantity= " + dto.getQuantity());
		System.out.println("cdtyCode= " + dto.getCommodityCode());
		System.out.println("mktCode= " + dto.getMarketCode());
		System.out.println("tradingPeriod= " + dto.getTradingPeriod());
		System.out.println("formulaName= " + dto.getFormulaName());
		System.out.println("transId= " + dto.getTransactionId());
		System.out.println("priceQty= " + dto.getPriceQty());
		System.out.println("priceQtyUom= " + dto.getPriceQtyUom());
		System.out.println("secPriceQty= " + dto.getSecPriceQty());
		System.out.println("setPriceUOM= " + dto.getSecPriceUom());
		System.out.println("-----------end-----------");
	}

}
