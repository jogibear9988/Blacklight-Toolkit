<%@ Page Language="C#" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["url"]))
            Response.Write(
                (new System.IO.StreamReader(
                    ((System.Net.HttpWebResponse)(
                        (System.Net.HttpWebRequest)System.Net.WebRequest.Create(
                            Request.QueryString["url"])
                        ).GetResponse()
                    ).GetResponseStream())
                ).ReadToEnd().Replace(
                    "encoding=\"ISO-8859-1\"", "encoding=\"UTF-8\""
                )
            );
    }
</script>
