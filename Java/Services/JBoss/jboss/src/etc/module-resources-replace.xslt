<?xml version='1.0' encoding='UTF-8'?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xalan="http://xml.apache.org/xalan"
                xmlns:m="urn:jboss:module:1.1"
                xmlns="urn:jboss:module:1.1"
                exclude-result-prefixes="m xalan">

    <xsl:output omit-xml-declaration="yes" method="xml" encoding="UTF-8" indent="yes" xalan:indent-amount="4"/>
    <xsl:strip-space elements="*"/>
    <!-- files = a ';' separated list of versioned archives -->
    <xsl:param name="files"/>

    <!-- Fix up the module.xml to have version numbers -->

    <xsl:template match="@*|node()">
        <xsl:copy>
            <xsl:apply-templates select="@*|node()"/>
        </xsl:copy>
    </xsl:template>

    <!-- Write out a list of files for loading by ant -->
    <xsl:template match="m:resources">
        <xsl:copy>
            <!-- Copy any attributes on the matching tag -->
            <xsl:copy-of select="@*"/>

            <xsl:call-template name="split">
                <xsl:with-param name="list" select="$files"/>
            </xsl:call-template>

        </xsl:copy>
    </xsl:template>

    <xsl:template name="split">
        <xsl:param name="list" select="''"/>
        <xsl:param name="separator" select="';'"/>

        <xsl:if test="not($list = '' or $separator = '')">
            <xsl:variable name="head" select="substring-before(concat($list, $separator), $separator)"/>
            <xsl:variable name="tail" select="substring-after($list, $separator)"/>

            <resource-root path="{$head}"/>

            <xsl:call-template name="split">
                <xsl:with-param name="list" select="$tail"/>
                <xsl:with-param name="separator" select="$separator"/>
            </xsl:call-template>
        </xsl:if>
    </xsl:template>

</xsl:stylesheet>