using RabbitMQ.Client;
namespace NSRabbit_test {
    static class QueueUtil {
#if CUSTOM_RABBIT_PORT
        const string OTHER_HOST = "WhiteWabbit.icts.local";
        const string CUSTOM_HOST = "172.16.143.199";
        const int CUSTOM_PORT = 5672;
        //        const int CUSTOM_PORT2 = -1;
#endif
        const string AMP_PREFIX = "amqp://";
        const string QUSER = "amqp://";
        const string QPASS = "amqp://";

        internal static ConnectionFactory createFactory() {
            return createFactory(false);
        }
        internal static ConnectionFactory createFactory(bool useEndpoint) {
            ConnectionFactory icf = new ConnectionFactory();
//            AmqpTcpEndpoint endpoint;
            string content;

            if (useEndpoint) {
                content = AMP_PREFIX +
                    (!(string.IsNullOrEmpty(QUSER) || string.IsNullOrEmpty(QPASS)) ? (QUSER + ":" + QPASS) : string.Empty) + CUSTOM_HOST +
                    (CUSTOM_PORT > 0 ? (":"+CUSTOM_PORT.ToString()) : string.Empty);
                icf.Endpoint = new AmqpTcpEndpoint(content);
            } else {
#if CUSTOM_RABBIT_PORT
                icf.HostName = CUSTOM_HOST;
                if (CUSTOM_PORT > 0)
                    icf.Port = CUSTOM_PORT;
#else
            icf.HostName = "localhost";
#endif
            }
            return icf;
        }

        internal static ConnectionFactory createFactory20() {
            ConnectionFactory icf = new ConnectionFactory();
#if CUSTOM_RABBIT_PORT
            icf.HostName = OTHER_HOST;
            if (CUSTOM_PORT > 0)
                icf.Port = CUSTOM_PORT;
#else
            icf.HostName = "localhost";
#endif
            return icf;
        }

    }
}