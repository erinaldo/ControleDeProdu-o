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

namespace InfinitySolutions.Commom
{
    public class ISEmail
    {
        public static Retorno EnviarErro(Erro erro)
        {
            try
            {
                using (Bitmap bgTopo = new Bitmap(System.Environment.CurrentDirectory + @"\Logo.png"))
                {
                    LinkedResource lrTopo = ConverteBitmapToLinkedResource(bgTopo, ImageFormat.Png, "bgTopo");
                    StringBuilder sbCorpoEmail = new StringBuilder();

                    sbCorpoEmail.Append("<html>");
                    sbCorpoEmail.Append("<head>");
                    sbCorpoEmail.Append("<body style=' background-color: Aqua'>");
                    sbCorpoEmail.Append("<p style='color: blue' >");
                    sbCorpoEmail.Append("RELATÓRIO DE ERRO - PLAY SERVICE!");
                    sbCorpoEmail.Append("</p>");
                    sbCorpoEmail.Append("</head>");
                    sbCorpoEmail.Append("<div>");

                    sbCorpoEmail.Append("<table>");
                    sbCorpoEmail.Append("<tr>");
                    sbCorpoEmail.Append("<td>");
                    sbCorpoEmail.Append("<img id=\"imgTopo\" alt=\"Administrador\" src=\"cid:bgTopo\" />");
                    sbCorpoEmail.Append("</td>");
                    sbCorpoEmail.Append("<td>");
                    sbCorpoEmail.Append("<p>");
                    sbCorpoEmail.Append("ERRO EM PLAY SERVICE!");
                    sbCorpoEmail.Append("</p>");
                    sbCorpoEmail.Append("</td>");
                    sbCorpoEmail.Append("</tr>");
                    sbCorpoEmail.Append("</table>");

                    sbCorpoEmail.Append("<table>");
                    sbCorpoEmail.Append("<tr>");
                    sbCorpoEmail.Append("<td>");
                    sbCorpoEmail.Append("<p style='color: blue'>");
                    sbCorpoEmail.Append("CASO DE USO: ");
                    sbCorpoEmail.Append("</p>");
                    sbCorpoEmail.Append("</td>");
                    sbCorpoEmail.Append("<td>");
                    sbCorpoEmail.Append("<p>");
                    sbCorpoEmail.Append(erro.CasoDeUso.ToString());
                    sbCorpoEmail.Append("</p>");
                    sbCorpoEmail.Append("</td>");
                    sbCorpoEmail.Append("</tr>");

                    sbCorpoEmail.Append("<tr>");
                    sbCorpoEmail.Append("<td>");
                    sbCorpoEmail.Append("<p style='color: blue'>");
                    sbCorpoEmail.Append("FUNCIONALIDADE: ");
                    sbCorpoEmail.Append("</p>");
                    sbCorpoEmail.Append("</td>");
                    sbCorpoEmail.Append("<td>");
                    sbCorpoEmail.Append("<p>");
                    sbCorpoEmail.Append(erro.Funcionalidade.ToString());
                    sbCorpoEmail.Append("</p>");
                    sbCorpoEmail.Append("</td>");
                    sbCorpoEmail.Append("</tr>");

                    sbCorpoEmail.Append("<tr>");
                    sbCorpoEmail.Append("<td>");
                    sbCorpoEmail.Append("<p style='color: blue'>");
                    sbCorpoEmail.Append("CAMADA: ");
                    sbCorpoEmail.Append("</p>");
                    sbCorpoEmail.Append("</td>");
                    sbCorpoEmail.Append("<td>");
                    sbCorpoEmail.Append("<p>");
                    sbCorpoEmail.Append(erro.Camada.ToString());
                    sbCorpoEmail.Append("</p>");
                    sbCorpoEmail.Append("</td>");
                    sbCorpoEmail.Append("</tr>");

                    sbCorpoEmail.Append("<tr>");
                    sbCorpoEmail.Append("<td>");
                    sbCorpoEmail.Append("<p style='color: blue'>");
                    sbCorpoEmail.Append("ERRO: ");
                    sbCorpoEmail.Append("</p>");
                    sbCorpoEmail.Append("</td>");
                    sbCorpoEmail.Append("<td>");
                    sbCorpoEmail.Append("<p>");
                    sbCorpoEmail.Append(erro.Descricao);
                    sbCorpoEmail.Append("</p>");
                    sbCorpoEmail.Append("</td>");
                    sbCorpoEmail.Append("</tr>");

                    sbCorpoEmail.Append("<tr>");
                    sbCorpoEmail.Append("<td>");
                    sbCorpoEmail.Append("<p style='color: blue'>");
                    sbCorpoEmail.Append("LOCAL: ");
                    sbCorpoEmail.Append("</p>");
                    sbCorpoEmail.Append("</td>");
                    sbCorpoEmail.Append("<td>");
                    sbCorpoEmail.Append("<p>");
                    sbCorpoEmail.Append(erro.Entidade);
                    sbCorpoEmail.Append("</p>");
                    sbCorpoEmail.Append("</td>");
                    sbCorpoEmail.Append("</tr>");

                    sbCorpoEmail.Append("<tr>");
                    sbCorpoEmail.Append("<td>");
                    sbCorpoEmail.Append("<p style='color: blue'>");
                    sbCorpoEmail.Append("DATA/HORA: ");
                    sbCorpoEmail.Append("</p>");
                    sbCorpoEmail.Append("</td>");
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
                            mail.From = new MailAddress("caiocesar.vieira@hotmail.com", "CAIO VIEIRA");
                            mail.To.Add(new MailAddress("caiocesar.vieira@hotmail.com", "CAIO VIEIRA"));
                            //mail.To.Add(new MailAddress("felipeghi2@yahoo.com.br", "FELIPE ARRIGHI"));
                            mail.AlternateViews.Add(viewHtml);
                            mail.Subject = "RELATÓRIO DE ERRO !";
                            mail.Attachments.Add(new Attachment(Environment.CurrentDirectory + @"\" + erro.Imagem + ".png"));
                            mail.IsBodyHtml = true;
                            mail.BodyEncoding = Encoding.UTF8;

                            using (SmtpClient client = new SmtpClient("smtp.live.com"))
                            {
                                client.Credentials = new NetworkCredential("caiocesar.vieira@hotmail.com", "Joleny123");
                                client.EnableSsl = true;
                                client.Port = 25;
                                client.Send(mail);
                            }
                        }
                        return new Retorno(true);
                    }
                }
            }
            catch (Exception ex) { return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "ENVIAR ERRO")); }
        }

        private static LinkedResource ConverteBitmapToLinkedResource(Bitmap imagem, ImageFormat formato, string id)
        {
            MemoryStream stream = new MemoryStream();
            imagem.Save(stream, formato);
            stream.Seek(0, SeekOrigin.Begin);

            LinkedResource lkResource = new LinkedResource(stream);

            lkResource.ContentType.Name = id;
            lkResource.ContentId = id;

            return lkResource;
        }
    }
}
