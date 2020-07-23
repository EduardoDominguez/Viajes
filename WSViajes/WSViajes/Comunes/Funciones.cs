using System;
using System.IO;
using System.Web;
using Viajes.EL.Enum;

namespace WSViajes.Comunes
{
    public class Funciones
    {
        public static string uploadImagen(string pBase64, string pDirectorio, string pSubDirectorio, string pNombre, string pExtencion, string pCarpeta, string pComplementoRuta)
        {
            try
            {
                string strRuta = pComplementoRuta;
                if (!Directory.Exists(pDirectorio))
                {
                    Directory.CreateDirectory(pDirectorio);
                }

                if (!Directory.Exists(pSubDirectorio))
                {
                    Directory.CreateDirectory(pSubDirectorio);
                }

                if (!Directory.Exists(pCarpeta))
                {
                    Directory.CreateDirectory(pCarpeta);
                }

                if (String.IsNullOrEmpty(pNombre))
                    strRuta += $"{Guid.NewGuid().ToString()}.{pExtencion}";
                else
                    strRuta += $"{pNombre}.{pExtencion}";



                var workingDirectory = System.Web.Hosting.HostingEnvironment.MapPath($"~/{strRuta}");
                File.WriteAllBytes(workingDirectory, Convert.FromBase64String(pBase64.Substring(pBase64.IndexOf(",") + 1)));

                return strRuta;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string getExtensionImagenBasae64(string pImagen64)
        {
            string strExtensionDefault = "png";
            try
            {
                if (string.IsNullOrEmpty(pImagen64))
                    return strExtensionDefault;
                else
                {
                    int intPosicionInicial = pImagen64.IndexOf("/");
                    string strExtension = string.Empty;
                    strExtension = pImagen64.Substring(intPosicionInicial + 1, (pImagen64.IndexOf(";") - intPosicionInicial) - 1);
                    if (strExtension.Contains("svg") && strExtension.Contains("+xml"))
                        strExtension = strExtension.Substring(0, 3);

                    return strExtension;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return strExtensionDefault;
            }
        }

        public static void deleteExistingFile(string pRutaImagen)
        {
            try
            {
                var strRutaSinHost = pRutaImagen.Substring(pRutaImagen.IndexOf("/Assets") + 1).Replace("/", "\\");
                string rutaFisica = HttpContext.Current.Server.MapPath("/");
                File.Delete(@rutaFisica + strRutaSinHost);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static string GetMensajeCambioEstatus(int pIdEstatus, string pMensajePersonalizado)
        {
            
            try
            {
                string mensaje = string.Empty;
                switch (pIdEstatus)
                {
                    case (int) EstatusPedido.PorAsignar:
                        mensaje = "Tu orden se ha generado, estamos asignando la persona que te entregará.";
                    break;
                    case (int) EstatusPedido.Asignado:
                        mensaje = "Hemos asignado un repartidor para tu orden.";
                        break;
                    case (int) EstatusPedido.EnViaje:
                        mensaje = "El repartidor ya se encuentra en camino a recoger tu pedido.";
                        break;
                    case (int) EstatusPedido.ConProducto:
                        mensaje = "Hemos recogido tu pedido y estamos en camino a tu domicilio.";
                    break;
                    case (int) EstatusPedido.Terminado:
                        mensaje = "Tu orden ha sido entregada. Gracias por confiar en nosotros.";
                    break;
                    case (int)EstatusPedido.Calificado:
                        mensaje = "Gracias por calificar tu pedido, tu opinión es muy importante para seguir mejorando o ofrecerte un mejor servicio.";
                        break;
                    case (int)EstatusPedido.Cancelado:
                        mensaje = "Tu orden ha sido cancelada. Si tienes alguna duda comunicate con nosotros.";
                        break;
                    case (int)EstatusPedido.AceptadoComercio:
                        mensaje = ".";
                        break;
                    case (int)EstatusPedido.PreparandoPedido:
                        mensaje = "El comercio se encuentra preparando el pedido, cuando esté listo se te notificará para que puedas recogerlo.";
                        break;
                    case (int)EstatusPedido.EnCamino:
                        mensaje = "Hemos recogido tu pedido y estamos en camino a tu domicilio.";
                        break;
                }

                return mensaje;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }
    }
}