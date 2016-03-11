package aff.confirm.webservices.tradegateway.exception;


import javax.ws.rs.core.Response;
import javax.ws.rs.ext.ExceptionMapper;
import javax.ws.rs.ext.Provider;

@Provider
public class GatewayExceptionMapper implements ExceptionMapper<GatewayException> {


    @Override
    public Response toResponse(GatewayException e) {
        String msg = e.getMessage();
        StringBuilder sb = new StringBuilder("<error>");
        sb.append("<code>1</code>");
        sb.append("<message>" + msg + "</message>");
        sb.append("</error>");
        return Response.serverError().entity(sb.toString()).build();

    }

}
