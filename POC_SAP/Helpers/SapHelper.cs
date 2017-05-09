using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace POC_SAP.Helpers
{
    public class SapHelper : IDisposable
    {
        private RfcDestination _rfcDestination;
        public RfcDestination RfcDestination { get { return _rfcDestination; } set { _rfcDestination = value; } }
        public SapHelper(string destinationName)
        {
             _rfcDestination = RfcDestinationManager.GetDestination(destinationName);
        }
        //if only one destination configured on webconfig
        public SapHelper()
        {
            _rfcDestination = RfcDestinationManager.GetDestination(ConfigurationManager.AppSettings["SAP_NAME"]);
        }
        public bool TestConnection()
        {
            bool result = false;

            if (_rfcDestination != null)
            {
                try
                {
                    _rfcDestination.Ping();
                    result = true;
                }
                catch (RfcCommunicationException ex)
                {
                    result = false;
                }
            }
            return result;
        }

        public DataTable ConvertToTable(IRfcTable rfcTable)
        {
            DataTable result = new DataTable();
            for (int columnIndex = 0; columnIndex < rfcTable.ElementCount; columnIndex++)
            {
                RfcElementMetadata metadata = rfcTable.GetElementMetadata(columnIndex);
                result.Columns.Add(metadata.Name);

            }
            foreach (IRfcStructure row in rfcTable)
            {
                DataRow dr = result.NewRow();
                for (int colIndex = 0; colIndex < rfcTable.ElementCount; colIndex++)
                {
                    RfcElementMetadata metadata = rfcTable.GetElementMetadata(colIndex);
                    switch (metadata.DataType)
                    {
                        case RfcDataType.CHAR:
                            dr[colIndex] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.BYTE:
                            dr[colIndex] = row.GetByte(metadata.Name);
                            break;
                        case RfcDataType.NUM:
                            dr[colIndex] = row.GetDecimal(metadata.Name);
                            break;
                        case RfcDataType.BCD:
                            dr[colIndex] = row.GetDecimal(metadata.Name);
                            break;
                        case RfcDataType.DATE:
                            dr[colIndex] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.TIME:
                            dr[colIndex] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.UTCLONG:
                            dr[colIndex] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.UTCSECOND:
                            dr[colIndex] = row.GetLong(metadata.Name);
                            break;
                        case RfcDataType.UTCMINUTE:
                            dr[colIndex] = row.GetLong(metadata.Name);
                            break;
                        case RfcDataType.DTDAY:
                            dr[colIndex] = row.GetLong(metadata.Name);
                            break;
                        case RfcDataType.DTWEEK:
                            dr[colIndex] = row.GetLong(metadata.Name);
                            break;
                        case RfcDataType.DTMONTH:
                            dr[colIndex] = row.GetLong(metadata.Name);
                            break;
                        case RfcDataType.TSECOND:
                            dr[colIndex] = row.GetLong(metadata.Name);
                            break;
                        case RfcDataType.TMINUTE:
                            dr[colIndex] = row.GetLong(metadata.Name);
                            break;
                        case RfcDataType.CDAY:
                            dr[colIndex] = row.GetLong(metadata.Name);
                            break;
                        case RfcDataType.FLOAT:
                            dr[colIndex] = row.GetFloat(metadata.Name);
                            break;
                        case RfcDataType.INT1:
                            dr[colIndex] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.INT2:
                            dr[colIndex] = row.GetDecimal(metadata.Name);
                            break;
                        case RfcDataType.INT4:
                            dr[colIndex] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.INT8:
                            dr[colIndex] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.DECF16:
                            dr[colIndex] = row.GetDecimal(metadata.Name);
                            break;
                        case RfcDataType.DECF34:
                            dr[colIndex] = row.GetDecimal(metadata.Name);
                            break;
                        case RfcDataType.STRING:
                            dr[colIndex] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.XSTRING:
                            dr[colIndex] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.STRUCTURE:
                            break;
                        case RfcDataType.TABLE:
                            break;
                        case RfcDataType.CLASS:
                            break;
                        case RfcDataType.UNKNOWN:
                            break;
                        default:
                            break;
                    }
                }
                result.Rows.Add(dr);
            }
            return result;
        }

        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                RfcDestination = null;
            }
            disposed = true;
        }
    }
}