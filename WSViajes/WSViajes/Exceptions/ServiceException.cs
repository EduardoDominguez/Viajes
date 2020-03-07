using System;
using System.Runtime.Serialization;

namespace WSViajes.Exceptions
{
    public class ServiceException : System.ApplicationException
    {
        /// <summary>
        /// Constructor de la Clase (Implementación 1)
        /// </summary>
        public ServiceException() : base(string.Empty)
        {

        }

        /// <summary>
        /// Constructor de la Clase (Implementación 2)
        /// </summary>
        /// <param name="new_message">Nuevo mensaje que será asignado a la excepción</param>
        /// <param name="new_errorcode">Código del Error</param>
        public ServiceException(string new_message, int new_errorcode)
            : base(new_message)
        {
            HResult = new_errorcode;
        }

        /// <summary>
        /// Constructor de la Clase (Implementación 3)
        /// </summary>
        /// <param name="new_message">Nuevo mensaje que será asignado a la excepción</param>
        /// <param name="new_errorcode">Código del Error</param>
        public ServiceException(string new_message, int new_errorcode, Exception inner_exception)
            : base(new_message, inner_exception)
        {
            HResult = new_errorcode;
        }

        /// <summary>
        /// Constructor de la Clase (Implementación 4)
        /// </summary>
        /// <param name="new_message">Nuevo mensaje que será asignado a la excepción</param>
        /// <param name="inner_exception">Un System.Exception que contiene la excepción interna de la cual deriva la actual.</param>
        public ServiceException(string new_message, Exception inner_exception)
            : base(new_message, inner_exception)
        {

        }

        /// <summary>
        /// Constructor de la Clase (Implementación 5)
        /// </summary>
        /// <param name="info" >Un System.Runtime.Serialization.SerializationInfo que contiene los datos necesarios para serializar o desserializar un objeto.</param>
        /// <param name="context" >Un  System.Runtime.Serialization.StreamingContext que describe el origen y destino de un determiando stream serializado.</param>
        public ServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        /// <summary>
        /// Devuelve el Código del Error.
        /// </summary>
        public int Codigo
        {
            get { return HResult; }
        }


    }//Class.

}//Namespace.
