using System;
//using System.Xml;

namespace XNALib
{
    public class CaseStatementMissingException : Exception
    {
        public CaseStatementMissingException()
            : base()
        {
        }

        public CaseStatementMissingException(string message)
            : base(message)
        {
        }
    }
    public class IfStatementMissingException : Exception
    {
        public IfStatementMissingException()
            : base()
        {
        }
    }

    public class NullException : Exception
    {
        public NullException()
            : base()
        {
        }
    }
    /*
    public class XMLNodeIsNullException : Exception
    {
        public XMLNodeIsNullException()
            : base()
        {
        }
        public XMLNodeIsNullException(XmlNode xmlNode)
            : base()
        {
        }
    }*/
}
