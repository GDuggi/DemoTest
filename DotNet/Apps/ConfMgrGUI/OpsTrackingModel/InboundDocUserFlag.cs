using System;
using System.Collections.Generic;
using System.Text;

namespace OpsTrackingModel
{
    [NHibernate.Mapping.Attributes.Class(Table = "INBOUND_DOC_USER_FLAG")]
    public class InboundDocsUserFlags
    {
        public static string BOOKMARK = "BOOKMARK";
        public static string COMMENT = "COMMENT";
        public static string IGNORE = "IGNORE";
        public static string CLEAR = "";

        private Int32 _id = 0;
        private Int32 _inboundDocsId;
        private string _inboundUser;
        private string _flagType;
        private string _comments;

        /// <summary> Gets the unique identifier. </summary>
        [NHibernate.Mapping.Attributes.Id(0, Name = "Id", Column = "ID")]
            [NHibernate.Mapping.Attributes.Generator(1, Class = "sequence")]
        [NHibernate.Mapping.Attributes.Param(2, Name = "sequence", Content = "SEQ_INBOUND_DOC_USER_FLAG")]
        public virtual Int32 Id
        {
            get { return _id; }
        }

        /// <summary> Parent Document ID </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "INBOUND_DOC_ID")]
        public virtual Int32 InboundDocsId
        {
            get { return _inboundDocsId; }
            set { _inboundDocsId = value; }
        }

        /// <summary> Inbound User Name. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "INBOUND_USER")]
        public virtual string InboundUser
        {
            get { return _inboundUser; }
            set { _inboundUser = value; }
        }

        /// <summary> Inbound User Flag Type. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = true, Column = "FLAG_TYPE")]
        public virtual string FlagType
        {
            get { return _flagType; }
            set { _flagType = value; }
        }

        /// <summary> Inbound User Flag Type. </summary>
        [NHibernate.Mapping.Attributes.Property(NotNull = false, Column = "COMMENTS")]
        public virtual string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        /// <summary> Default constructor. </summary>
        public InboundDocsUserFlags()
        {
        }
    }
}
