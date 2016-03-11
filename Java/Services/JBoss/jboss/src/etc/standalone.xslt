<?xml version='1.0' encoding='UTF-8'?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xalan="http://xml.apache.org/xalan"
                xmlns:jb="urn:jboss:domain:1.5"
                xmlns:ds="urn:jboss:domain:datasources:1.1"
                xmlns:hq="urn:jboss:domain:messaging:1.4"
                xmlns:mail="urn:jboss:domain:mail:1.1"
                exclude-result-prefixes="jb hq ds mail xalan">

    <xsl:output omit-xml-declaration="yes" method="xml" encoding="UTF-8" indent="yes" xalan:indent-amount="4"/>
    <xsl:strip-space elements="*"/>

    <!-- Copy everything -->
    <xsl:template match="@*|node()">
        <xsl:copy>
            <xsl:apply-templates select="@*|node()"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="jb:extensions">
        <!-- Copy everything -->
        <xsl:copy-of select="."/>

        <!-- Append after extensions -->
        <system-properties xmlns="urn:jboss:domain:1.5">
            <property name="aff.sif.aqjmsadapter.AqJmsAdapter.configFile" value="${{jboss.server.config.dir}}/aff-sif-AqJmsAdapter.xml"/>
            <property name="DBInfo.DatabaseName" value="cnf_mgr"/>
            <property name="DBInfo.DBUrl" value="jdbc:sqlserver://CNF01INFDBS01\SQLSVR11;databaseName=cnf_mgr"/>
            <property name="DBInfo.DBUserName" value="cnfmgrsvc"/>
            <property name="DBInfo.DBPassword" value="6y5t^Y%T"/>
            <property name="cnf.docflow.mdb.MercuriaTradeAlertProcessorMDB.EmailNotify" value="jvega@amphorainc.com"/>
            <property name="cnf.docflow.mdb.MercuriaTradeAlertProcessorMDB.EmailDomain" value="@amphorainc.com"/>
            <property name="cnf.docflow.mdb.MercuriaTradeAlertProcessorMDB.DebugLogging" value="TRUE"/>
            <property name="cnf.docflow.mdb.MercuriaTradeAlertProcessorMDB.MaxDeliveryAttemptsforEmail" value="10"/>

            <property name="aff.db.confirm.datasource" value="java:jboss/datasources/Aff.SqlSvr.DS"/>

            <property name="aff.cnf.jms.server" value="localhost"/>
            <property name="aff.cnf.jms.user" value="sempra.ops.jboss"/>
            <property name="aff.cnf.jms.password" value="sempra"/>
            <property name="aff.cnf.jms.fail.queue.name" value="confirmsMgr.tradeNotification.dbLogger.failed"/>
            <property name="aff.cnf.jms.message.failed.queue.expiration.hours" value="24"/>
            <property name="aff.cnf.server.remoting.user.name" value="admin"/>
            <property name="aff.cnf.server.remoting.user.password" value="Amphora-123"/>
            <property name="confirmationsManagerWebServiceURL" value="http://cnf01inf01:11111/ConfirmationsManager"/>


        </system-properties>
    </xsl:template>

    <xsl:template match="ds:subsystem/ds:datasources">
        <!-- Copy any attributes on the matching tag -->
        <xsl:copy-of select="@*"/>

        <!-- Replace this whole section -->
        <datasources xmlns="urn:jboss:domain:datasources:1.1">
            <datasource jta="false" jndi-name="java:jboss/datasources/aff-sif-AqAdapterDs" pool-name="aff-sif-AqAdapterDs" enabled="true" use-ccm="false">
                <connection-url>jdbc:sqlserver://CNF01INFDBS01\SQLSVR11;databaseName=cnf_mgr</connection-url>
                <driver-class>com.microsoft.sqlserver.jdbc.SQLServerDriver</driver-class>
                <driver>sqlserver</driver>
                <pool>
                    <min-pool-size>0</min-pool-size>
                    <max-pool-size>10</max-pool-size>
                    <prefill>true</prefill>
                </pool>
                <security>
                    <user-name>AFFMSG</user-name>
                    <password>AFFMSG</password>
                </security>
                <validation>
                    <validate-on-match>false</validate-on-match>
                    <background-validation>false</background-validation>
                </validation>
                <statement>
                    <share-prepared-statements>false</share-prepared-statements>
                </statement>
            </datasource>
            <datasource jndi-name="java:jboss/datasources/Aff.SqlSvr.DS" pool-name="Aff.SqlSvr.DS" enabled="true" use-java-context="true">
                <connection-url>jdbc:sqlserver://CNF01INFDBS01\SQLSVR11;databaseName=cnf_mgr</connection-url>
                <driver-class>com.microsoft.sqlserver.jdbc.SQLServerDriver</driver-class>
                <driver>sqlserver</driver>
                <pool>
                    <max-pool-size>20</max-pool-size>
                </pool>
                <security>
                    <user-name>cnfmgrsvc</user-name>
                    <password>6y5t^Y%T</password>
                </security>
            </datasource>
            <drivers>
                <driver name="sqlserver" module="aff.com-microsoft-sqlserver-jdbc">
                    <xa-datasource-class>com.microsoft.sqlserver.jdbc.SQLServerDataSource</xa-datasource-class>
                </driver>
            </drivers>
        </datasources>
    </xsl:template>

    <xsl:template match="mail:mail-session">
        <mail-session xmlns="urn:jboss:domain:mail:1.1" jndi-name="java:/Mail">
            <xsl:copy-of select="child::node()"/>
        </mail-session>
    </xsl:template>


    <xsl:template match="hq:subsystem">
        <xsl:copy>
            <!-- Replace this whole section -->
            <hornetq-server xmlns="urn:jboss:domain:messaging:1.4">
                <persistence-enabled>true</persistence-enabled>
                <message-counter-enabled>true</message-counter-enabled>
                <jmx-management-enabled>true</jmx-management-enabled>
                <journal-type>NIO</journal-type>
                <journal-file-size>102400</journal-file-size>
                <journal-min-files>2</journal-min-files>
                <connectors>
                    <netty-connector name="netty" socket-binding="messaging"/>
                    <netty-connector name="netty-throughput" socket-binding="messaging-throughput">
                        <param key="batch-delay" value="50"/>
                    </netty-connector>
                    <in-vm-connector name="in-vm" server-id="0"/>
                </connectors>
                <acceptors>
                    <netty-acceptor name="netty" socket-binding="messaging"/>
                    <netty-acceptor name="netty-throughput" socket-binding="messaging-throughput">
                        <param key="batch-delay" value="50"/>
                        <param key="direct-deliver" value="false"/>
                    </netty-acceptor>
                    <netty-acceptor name="stomp-acceptor" socket-binding="messaging-stomp">
                        <param key="protocol" value="stomp"/>
                        <param key="connection-ttl" value="1200000"/>
                    </netty-acceptor>
                    <in-vm-acceptor name="in-vm" server-id="0"/>
                </acceptors>

                <security-settings>
                    <security-setting match="#">
                        <permission type="send" roles="guest"/>
                        <permission type="consume" roles="guest"/>
                        <permission type="createNonDurableQueue" roles="guest"/>
                        <permission type="deleteNonDurableQueue" roles="guest"/>
                    </security-setting>
                </security-settings>

                <address-settings>

                    <!--default for catch all-->
                    <address-setting match="#">
                        <dead-letter-address>jms.queue.DLQ</dead-letter-address>
                        <expiry-address>jms.queue.ExpiryQueue</expiry-address>
                        <redelivery-delay>0</redelivery-delay>
                        <max-size-bytes>10485760</max-size-bytes>
                        <address-full-policy>PAGE</address-full-policy>
                        <page-size-bytes>5242880</page-size-bytes>
                        <page-max-cache-size>5</page-max-cache-size>
                        <message-counter-history-day-limit>1</message-counter-history-day-limit>
                    </address-setting>
                </address-settings>

                <jms-connection-factories>
                    <connection-factory name="InVmConnectionFactory">
                        <connectors>
                            <connector-ref connector-name="in-vm"/>
                        </connectors>
                        <entries>
                            <entry name="java:/ConnectionFactory"/>
                        </entries>
                        <connection-ttl>-1</connection-ttl>
                        <client-failure-check-period>600000</client-failure-check-period>
                        <reconnect-attempts>-1</reconnect-attempts>
                    </connection-factory>
                    <pooled-connection-factory name="hornetq-ra">
                        <transaction mode="xa"/>
                        <connectors>
                            <connector-ref connector-name="in-vm"/>
                        </connectors>
                        <entries>
                            <entry name="java:/JmsXA"/>
                        </entries>
                        <connection-ttl>-1</connection-ttl>
                        <client-failure-check-period>600000</client-failure-check-period>
                        <reconnect-attempts>-1</reconnect-attempts>
                    </pooled-connection-factory>
                    <connection-factory name="RemoteConnectionFactory">
                        <connectors>
                            <connector-ref connector-name="netty"/>
                        </connectors>
                        <entries>
                            <entry name="java:jboss/exported/jms/RemoteConnectionFactory"/>
                        </entries>
                        <retry-interval>5000</retry-interval>
                        <retry-interval-multiplier>1.5</retry-interval-multiplier>
                        <max-retry-interval>30000</max-retry-interval>
                        <reconnect-attempts>-1</reconnect-attempts>
                        <connection-ttl>60000</connection-ttl>
                        <client-failure-check-period>30000</client-failure-check-period>
                        <confirmation-window-size>-1</confirmation-window-size>
                    </connection-factory>
                    <connection-factory name="RemoteNoCacheConnectionFactory">
                        <connectors>
                            <connector-ref connector-name="netty"/>
                        </connectors>
                        <entries>
                            <entry name="java:jboss/exported/jms/RemoteNoCacheConnectionFactory"/>
                        </entries>
                        <retry-interval>5000</retry-interval>
                        <retry-interval-multiplier>1.5</retry-interval-multiplier>
                        <max-retry-interval>30000</max-retry-interval>
                        <reconnect-attempts>-1</reconnect-attempts>
                        <connection-ttl>60000</connection-ttl>
                        <client-failure-check-period>30000</client-failure-check-period>
                        <confirmation-window-size>-1</confirmation-window-size>
                        <consumer-window-size>0</consumer-window-size>
                    </connection-factory>
                </jms-connection-factories>

                <!-- Replace this whole section -->
                <jms-destinations xmlns="urn:jboss:domain:messaging:1.4">
                    <jms-queue name="confirmsMgr.tradeNotification.dbLogger">
                        <entry name="queue/confirmsMgr.tradeNotification.dbLogger"/>
                        <entry name="java:jboss/exported/jms/queue/confirmsMgr.tradeNotification.dbLogger"/>
                    </jms-queue>
                    <jms-queue name="confirmsMgr.tradeNotification.dbLogger.failed">
                        <entry name="queue/confirmsMgr.tradeNotification.dbLogger.failed"/>
                        <entry name="java:jboss/exported/jms/queue/confirmsMgr.tradeNotification.dbLogger.failed"/>
                    </jms-queue>
					<jms-queue name="confirmsMgr.tradeAppr.activityAlert">
                        <entry name="queue/confirmsMgr.tradeAppr.activityAlert"/>
                        <entry name="java:jboss/exported/jms/queue/confirmsMgr.tradeAppr.activityAlert"/>
                    </jms-queue>
                    <jms-queue name="confirmsMgr.tradeRqmt.statusChange">
                        <entry name="queue/confirmsMgr.tradeRqmt.statusChange"/>
                        <entry name="java:jboss/exported/jms/queue/confirmsMgr.tradeRqmt.statusChange"/>
                    </jms-queue>
                    <jms-queue name="testQueue">
                        <entry name="queue/test"/>
                        <entry name="java:jboss/exported/jms/queue/test"/>
                    </jms-queue>
                    <jms-queue name="mercuria.confirmsMgr.tradeNotification">
                        <entry name="queue/mercuria.confirmsMgr.tradeNotification"/>
                        <entry name="java:jboss/exported/jms/queue/mercuria.confirmsMgr.tradeNotification"/>
                    </jms-queue>
                    <jms-queue name="sempra.ops.associatedDocs.activityAlert">
                        <entry name="java:jboss/exported/jms/queue/sempra.ops.associatedDocs.activityAlert"/>
                        <entry name="queue/sempra.ops.associatedDocs.activityAlert"/>
                    </jms-queue>
                    <jms-queue name="sempra.ops.extMarginNotify">
                        <entry name="java:jboss/exported/jms/queue/sempra.ops.extMarginNotify"/>
                        <entry name="queue/sempra.ops.extMarginNotify"/>
                    </jms-queue>
                    <jms-queue name="sempra.ops.inboundDocs.activityAlert">
                        <entry name="java:jboss/exported/jms/queue/sempra.ops.inboundDocs.activityAlert"/>
                        <entry name="queue/sempra.ops.inboundDocs.activityAlert"/>
                    </jms-queue>
                    <jms-queue name="sempra.ops.opsTracking.activityAlert">
                        <entry name="java:jboss/exported/jms/queue/sempra.ops.opsTracking.activityAlert"/>
                        <entry name="queue/sempra.ops.opsTracking.activityAlert"/>
                    </jms-queue>
                    <jms-queue name="sempra.ops.tradeNotification.tradeData">
                        <entry name="java:jboss/exported/jms/queue/sempra.ops.tradeNotification.tradeData"/>
                        <entry name="queue/sempra.ops.tradeNotification.tradeData"/>
                    </jms-queue>
                    <jms-queue name="sempra.ops.tradeRqmtConfirm.activityAlert">
                        <entry name="java:jboss/exported/jms/queue/sempra.ops.tradeRqmtConfirm.activityAlert"/>
                        <entry name="queue/sempra.ops.tradeRqmtConfirm.activityAlert"/>
                    </jms-queue>
                    <jms-topic name="sempra.ops.opsTracking.associatedDocs.update">
                        <entry name="java:jboss/exported/jms/topic/sempra.ops.opsTracking.associatedDocs.update"/>
                        <entry name="topic/sempra.ops.opsTracking.associatedDocs.update"/>
                    </jms-topic>
                    <jms-topic name="sempra.ops.opsTracking.inboundDocs.update">
                        <entry name="java:jboss/exported/jms/topic/sempra.ops.opsTracking.inboundDocs.update"/>
                        <entry name="topic/sempra.ops.opsTracking.inboundDocs.update"/>
                    </jms-topic>
                    <jms-topic name="sempra.ops.opsTracking.rqmt.update">
                        <entry name="java:jboss/exported/jms/topic/sempra.ops.opsTracking.rqmt.update"/>
                        <entry name="topic/sempra.ops.opsTracking.rqmt.update"/>
                    </jms-topic>
                    <jms-topic name="sempra.ops.opsTracking.summary.update">
                        <entry name="java:jboss/exported/jms/topic/sempra.ops.opsTracking.summary.update"/>
                        <entry name="topic/sempra.ops.opsTracking.summary.update"/>
                    </jms-topic>
                    <jms-topic name="sempra.ops.opsTracking.tradeRqmtConfirm.update">
                        <entry name="java:jboss/exported/jms/topic/sempra.ops.opsTracking.tradeRqmtConfirm.update"/>
                        <entry name="topic/sempra.ops.opsTracking.tradeRqmtConfirm.update"/>
                    </jms-topic>
                </jms-destinations>
                <diverts>
                    <divert name="q:mercuria.confirmsMgr.tradeNotification">
                        <address>jms.queue.mercuria.confirmsMgr.tradeNotification</address>
                        <forwarding-address>jms.queue.confirmsMgr.tradeNotification.dbLogger</forwarding-address>
                        <exclusive>false</exclusive>
                    </divert>
                </diverts>

            </hornetq-server>
        </xsl:copy>
    </xsl:template>

    <!-- Add socket-binding before any outbound-socket-binding -->
    <xsl:template match="jb:socket-binding-group/jb:outbound-socket-binding">
        <socket-binding name="messaging-stomp" port="61613" xmlns="urn:jboss:domain:1.5"/>
        <xsl:copy-of select="."/>
    </xsl:template>

</xsl:stylesheet>