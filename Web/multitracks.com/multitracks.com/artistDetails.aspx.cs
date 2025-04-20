using System;
using System.Web.UI;
using System.Data;
using System.Text;
using DataAccess;
using Multitracks_Api.Models; // Add reference to the Models namespace to use TimeSignatureExtensions

public partial class artistDetails : Page
{
    // Properties to store artist information for the page
    public string ArtistName { get; set; }
    public string ArtistImageURL { get; set; }
    public string ArtistHeroURL { get; set; }
    public string ArtistBiography { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadArtistDetails();
        }
    }

    private void LoadArtistDetails()
    {
        // Get artistID from the query string
        int artistID;
        if (!int.TryParse(Request.QueryString["artistID"], out artistID))
        {
            // Default to artistID 1 if not provided or invalid
            artistID = 2;
        }

        // Use MTDataAccess to call the stored procedure
        SQL sql = new SQL();
        try
        {
            // Add the artistID parameter
            sql.Parameters.Add("@artistID", artistID);

            // Execute the stored procedure which returns multiple result sets
            DataSet ds = sql.ExecuteStoredProcedureDS("GetArtistDetails");

            if (ds != null && ds.Tables.Count >= 3)
            {
                // Process Artist Details (first result set)
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow artistRow = ds.Tables[0].Rows[0];
                    ArtistName = DB.Write<string>(ds.Tables[0], "title");
                    ArtistImageURL = DB.Write<string>(ds.Tables[0], "imageURL");
                    ArtistHeroURL = DB.Write<string>(ds.Tables[0], "heroURL");
                    ArtistBiography = FormatBiography(DB.Write<string>(ds.Tables[0], "biography"));
                }

                // Process time signature values before binding
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        // Get the time signature value
                        if (row["timeSignature"] != DBNull.Value && int.TryParse(row["timeSignature"].ToString(), out int timeSignatureValue))
                        {
                            // Convert numeric time signature to formatted display string (4/4 or 6/8)
                            row["timeSignature"] = TimeSignatureExtensions.GetDisplayString(timeSignatureValue);
                        }
                    }
                }

                // Bind Songs data (second result set)
                rptSongs.DataSource = ds.Tables[1];
                rptSongs.DataBind();

                // Bind Albums data (third result set)
                rptAlbums.DataSource = ds.Tables[2];
                rptAlbums.DataBind();
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., log the error)
            // For now, just show a message
            Response.Write("<script>alert('Error loading artist details: " + ex.Message + "');</script>");
        }
    }

    private string FormatBiography(string biography)
    {
        if (string.IsNullOrEmpty(biography))
            return "<p>No biography available.</p>";

        // Split the biography by paragraphs and format as HTML
        string[] paragraphs = biography.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        StringBuilder sb = new StringBuilder();
        
        foreach (string paragraph in paragraphs)
        {
            if (!string.IsNullOrWhiteSpace(paragraph))
            {
                sb.AppendFormat("<p>{0}</p>", paragraph.Trim());
            }
        }

        return sb.ToString();
    }

    // Helper method to render feature icons based on availability
    public string RenderFeatureIcon(bool isAvailable, string iconId, string cssClass)
    {
        if (!isAvailable)
            return string.Empty;

        return string.Format(@"
            <a href=""#"" class=""{0}"" style=""display: inline-block"">
                <svg class=""song-list--item--icon"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24"" id=""{1}"">
                    <path fill-rule=""evenodd"" clip-rule=""evenodd"" d=""M1.977 12c0-5.523 4.477-10 10-10s10 4.477 10 10-4.477 10-10 10-10-4.477-10-10zm4.04 6.204a8.579 8.579 0 005.96 2.405c4.747 0 8.61-3.862 8.61-8.609 0-4.746-3.863-8.609-8.61-8.609a8.573 8.573 0 00-5.893 2.343h6.598a.708.708 0 110 1.415H4.87c-.29.423-.543.875-.754 1.348h11.213a.708.708 0 110 1.415H3.624c-.109.437-.184.888-.223 1.35h14.637a.708.708 0 110 1.414H3.396c.037.46.109.911.215 1.348h13.025a.708.708 0 010 1.416H4.087c.207.473.454.923.739 1.349h9.164a.708.708 0 110 1.415H6.017z"" fill=""""></path>
                </svg>
            </a>", cssClass, iconId);
    }
}