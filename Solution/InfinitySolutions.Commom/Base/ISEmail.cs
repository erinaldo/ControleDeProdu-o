using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Net.Mail;
using System.IO;
using System.Drawing.Imaging;
using System.Net;
using InfinitySolutions.Entity;
using System.Configuration;
using System.Diagnostics;

namespace InfinitySolutions.Commom
{
    public class ISEmail
    {
        public static Retorno EnviarErro(string descricaoErro)
        {
            try
            {
                if (Internet.Conectado())
                {
                    using (Bitmap bgTopo = new Bitmap(System.Environment.CurrentDirectory + @"\LogoInfinitySolutions.png"))
                    {
                        LinkedResource lrTopo = ConverteBitmapToLinkedResource(bgTopo, ImageFormat.Png, "bgTopo");
                        StringBuilder sbCorpoEmail = new StringBuilder();

                        sbCorpoEmail.Append("<html>");
                        sbCorpoEmail.Append("<head>");
                        sbCorpoEmail.Append("</head>");
                        sbCorpoEmail.Append("<body>");
                        sbCorpoEmail.Append("<div>");

                        sbCorpoEmail.Append("<table>");
                        sbCorpoEmail.Append("<tr>");
                        sbCorpoEmail.Append("<td>");
                        sbCorpoEmail.Append("<img id=\"imgTopo\" alt=\"Administrador\" src=\"cid:bgTopo\" />");
                        sbCorpoEmail.Append("</td>");
                        sbCorpoEmail.Append("</tr>");

                        sbCorpoEmail.Append("<tr>");
                        sbCorpoEmail.Append("<td>");
                        sbCorpoEmail.Append("<p style='color: blue'>");
                        sbCorpoEmail.Append("ERRO: ");
                        sbCorpoEmail.Append("</p>");
                        sbCorpoEmail.Append("</td>");
                        sbCorpoEmail.Append("</tr>");
                        sbCorpoEmail.Append("<tr>");
                        sbCorpoEmail.Append("<td>");
                        sbCorpoEmail.Append("<p>");
                        sbCorpoEmail.Append(descricaoErro);
                        sbCorpoEmail.Append("</p>");
                        sbCorpoEmail.Append("</td>");
                        sbCorpoEmail.Append("</tr>");

                        sbCorpoEmail.Append("<tr>");
                        sbCorpoEmail.Append("<td>");
                        sbCorpoEmail.Append("<p>");
                        sbCorpoEmail.Append(DateTime.Now);
                        sbCorpoEmail.Append("</p>");
                        sbCorpoEmail.Append("</td>");
                        sbCorpoEmail.Append("</tr>");

                        sbCorpoEmail.Append("</table>");
                        sbCorpoEmail.Append("</div>");
                        sbCorpoEmail.Append("</body>");
                        sbCorpoEmail.Append("</html>");

                        using (AlternateView viewHtml = AlternateView.CreateAlternateViewFromString(sbCorpoEmail.ToString(), null, "text/html"))
                        {
                            viewHtml.LinkedResources.Add(lrTopo);

                            using (MailMessage mail = new MailMessage())
                            {
                                mail.From = new MailAddress("infinitysolutionserro@outlook.com", Configuracao.NOME_EMPRESA);
                                mail.To.Add(new MailAddress("infinitysolutionserro@outlook.com", Configuracao.NOME_EMPRESA));
                                mail.AlternateViews.Add(viewHtml);
                                mail.Subject = "RELATÓRIO DE ERRO - " + Configuracao.NOME_EMPRESA;

                                mail.IsBodyHtml = true;
                                mail.BodyEncoding = Encoding.UTF8;

                                using (SmtpClient client = new SmtpClient("smtp.live.com"))
                                {
                                    client.Credentials = new NetworkCredential("infinitysolutionserro@outlook.com", "Joleny123");
                                    client.EnableSsl = true;
                                    client.Port = 25;
                                    client.Send(mail);
                                }
                            }
                        }
                    }
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                ISEmail.EnviarErro(String.Format(Mensagens.MSG_26, ex.Message, "Na camada de dados ao ENVIAR BACKUP ISEmail"));

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "ENVIAR ERRO"));
            }
        }

        static LinkedResource ConverteBitmapToLinkedResource(Bitmap imagem, ImageFormat formato, string id)
        {
            MemoryStream stream = new MemoryStream();
            imagem.Save(stream, formato);
            stream.Seek(0, SeekOrigin.Begin);

            LinkedResource lkResource = new LinkedResource(stream);

            lkResource.ContentType.Name = id;
            lkResource.ContentId = id;

            return lkResource;
        }

        public static Retorno EnviarBackup()
        {
            try
            {
                using (Bitmap bgTopo = new Bitmap(System.Environment.CurrentDirectory + @"\LogoInfinitySolutions.png"))
                {
                    LinkedResource lrTopo = ConverteBitmapToLinkedResource(bgTopo, ImageFormat.Png, "bgTopo");
                    StringBuilder sbCorpoEmail = new StringBuilder();

                    sbCorpoEmail.Append("<table>");
                    sbCorpoEmail.Append("<tr>");
                    sbCorpoEmail.Append("<td>");
                    sbCorpoEmail.Append("<img id=\"imgTopo\" alt=\"INFINITY SOLUTIONS\" src=\"cid:bgTopo\" />");
                    sbCorpoEmail.Append("</td>");
                    sbCorpoEmail.Append("</tr>");
                    sbCorpoEmail.Append("</table>");

                    using (AlternateView viewHtml = AlternateView.CreateAlternateViewFromString(sbCorpoEmail.ToString(), null, "text/html"))
                    {
                        viewHtml.LinkedResources.Add(lrTopo);

                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("infinitysolutionsbackup@outlook.com", Configuracao.NOME_EMPRESA);
                            mail.To.Add(new MailAddress("infinitysolutionsbackup@outlook.com", ""));
                            mail.AlternateViews.Add(viewHtml);
                            mail.Subject = "BACKUP - " + Configuracao.NOME_EMPRESA;

                            Retorno retorno = Seguranca.GerarBackup();

                            if (retorno.IsValido)
                            {
                                mail.Attachments.Add(new Attachment(retorno.Entity.ToString()));

                                mail.IsBodyHtml = true;
                                mail.BodyEncoding = Encoding.UTF8;

                                using (SmtpClient client = new SmtpClient("smtp.live.com"))
                                {
                                    client.Credentials = new NetworkCredential("infinitysolutionsbackup@outlook.com", "Joleny123");
                                    client.EnableSsl = true;
                                    client.Port = 25;
                                    client.Send(mail);
                                }
                            }
                            else
                                return retorno;
                        }
                        return new Retorno(true);
                    }
                }
            }
            catch (Exception ex)
            {
                return new Retorno(false, "Ocorreu um erro ao enviar backup. Informe o Erro ao Suporte da INFINITY SOLUTIONS." + ex.Message);
            }
        }



        public static Retorno EnviarArquivosNfe(Pedido pedido, byte[] danfe, byte[] xml)
        {
            try
            {
                //LinkedResource lrTopo = ConverteBitmapToLinkedResource(bgTopo, ImageFormat.Png, numeroDocumento + ".png");
                StringBuilder sbCorpoEmail = new StringBuilder();

                sbCorpoEmail.Append("<html>");
                sbCorpoEmail.Append("<head>");
                sbCorpoEmail.Append("</head>");
                sbCorpoEmail.Append("   <body>");
                sbCorpoEmail.Append("      <div>");

                sbCorpoEmail.Append("          <table>");
                sbCorpoEmail.Append("              <tr>");
                sbCorpoEmail.Append("                  <td>");
                sbCorpoEmail.Append("                       Prezado(a) <br/>");
                sbCorpoEmail.Append("                       Segue em anexo os arquivos referente a NFE do PEDIDO " + pedido.Codigo.ToString("0000000") + " <br/>");
                sbCorpoEmail.Append("                       Cliente: " + pedido.Cliente.Nome + " <br/>");
                sbCorpoEmail.Append("<br/>");
                sbCorpoEmail.Append("                       Att;<br/>");
                sbCorpoEmail.Append("                  </td>");
                sbCorpoEmail.Append("              </tr>");
                sbCorpoEmail.Append("          </table>");

                sbCorpoEmail.Append("      </div>");
                sbCorpoEmail.Append("   </body>");
                sbCorpoEmail.Append("</html>");


                using (AlternateView viewHtml = AlternateView.CreateAlternateViewFromString(sbCorpoEmail.ToString(), null, "text/html"))
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        Stream stream = new MemoryStream(danfe);
                        Attachment anexoNfe = new Attachment(stream, "Danfe_Pedido" + pedido.Codigo.ToString("0000000") + ".pdf");
                        mail.Attachments.Add(anexoNfe);

                        stream = new MemoryStream(xml);
                        anexoNfe = new Attachment(stream, "Xml_Pedido" + pedido.Codigo.ToString("0000000") + ".xml");
                        mail.Attachments.Add(anexoNfe);

                        //mail.Headers.Add("Disposition-Notification-To", "infinitysolutionssystens@hotmail.com");
                        mail.From = new MailAddress("infinitysolutionssystens@hotmail.com", "Infinity Solutions Systens");
                        mail.To.Add(new MailAddress("caiocesar.vieira@hotmail.com", "Caio"));
                        mail.To.Add(new MailAddress("adm@vestiry.com.br", "Vestiry"));
                        mail.To.Add(new MailAddress("contato@vestiry.com.br", "Vestiry"));

                        mail.AlternateViews.Add(viewHtml);
                        mail.Subject = String.Format("[INFINITY SOLUTIONS SYSTENS] - Arquivos Pedido - {0}. {1}", pedido.Codigo.ToString("0000000"), pedido.Cliente.Nome);

                        mail.IsBodyHtml = true;
                        mail.BodyEncoding = Encoding.UTF8;
                        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

                        using (SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 465))
                        {
                            client.Port = 587;
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.UseDefaultCredentials = false;
                            client.EnableSsl = true;
                            client.TargetName = "smtp-mail.outlook.com";
                            client.Credentials = new NetworkCredential("infinitysolutionssystens@hotmail.com", "Joleny123");
                            client.Send(mail);
                        }
                    }
                }

                return new Retorno(true);
            }
            catch (Exception ex)
            {
                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "ENVIAR EMAIL BOLETO "));
            }
        }
    }
}
