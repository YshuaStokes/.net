<%@ Page Language="C#" AutoEventWireup="true" CodeFile="artistDetails.aspx.cs" Inherits="artistDetails" %>

<!DOCTYPE html>
<html lang="en">
<head>
	<!-- set the viewport width and initial-scale on mobile devices -->
	<meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=no" />

	<!-- set the encoding of your site -->
	<meta charset="utf-8">
	<title>MultiTracks.com - Artist Details</title>
	<!-- include the site stylesheet -->
	
	<link media="all" rel="stylesheet" href="<%=ResolveUrl("~/PageToSync/css/index.css")%>">
</head>
	<body class="premium standard u-fix-fancybox-iframe">
		<form id="form1" runat="server">
			<noscript>
				<div>Javascript must be enabled for the correct page display</div>
			</noscript>

			<!-- allow a user to go to the main content of the page -->
			<a class="accessibility" href="#main" tabindex="21">Skip to Content</a>

			<div class="wrapper mod-standard mod-gray">
				<div class="details-banner">
					<div class="details-banner--overlay"></div>
					<div class="details-banner--hero">
						<img class="details-banner--hero--img" src="<%=ArtistHeroURL%>" alt="<%=ArtistName%>">
					</div>
					<div class="details-banner--info">
						<a href="#" class="details-banner--info--box">
								<img class="details-banner--info--box--img"
								 src="<%=ArtistImageURL%>"
								 alt="<%=ArtistName%>">
						</a>
						<h1 class="details-banner--info--name"><a class="details-banner--info--name--link" href="#"><%=ArtistName%></a></h1>
					</div>
				</div>

				<nav class="discovery--nav">
					<ul class="discovery--nav--list tab-filter--list u-no-scrollbar">
						<li class="discovery--nav--list--item tab-filter--item is-active">
							<a class="tab-filter" href="../artists/details.aspx">Overview</a>
						</li>
						<li class="discovery--nav--list--item tab-filter--item">
							<a class="tab-filter" href="../artists/songs/details.aspx">Songs</a>
						</li>
						<li class="discovery--nav--list--item tab-filter--item">
							<a class="tab-filter" href="../artists/albums/details.aspx">Albums</a>
						</li>
					</ul> <!-- /.browse-header-filters -->
				</nav>

				<div class="discovery--container u-container">
							<main class="discovery--section">

								<section class="standard--holder">
									<div class="discovery--section--header">
										<h2>Top Songs</h2>
										<a class="discovery--section--header--view-all" href="#">View All</a>
									</div><!-- /.discovery-select -->

									<ul id="playlist" class="song-list mod-new mod-menu">
										<asp:Repeater ID="rptSongs" runat="server">
											<ItemTemplate>
												<li class="song-list--item media-player--row">
													<div class="song-list--item--player-img media-player">
														<img class="song-list--item--player-img--img" 
														src='<%# Eval("albumImageURL") %>' 
														alt='<%# Eval("songTitle") %>'>
													</div>
													<div class="song-list--item--right">
														<a href="#" class="song-list--item--primary"><%# Eval("songTitle") %></a>
														<a class="song-list--item--secondary"><%# Eval("albumTitle") %></a>
														<a class="song-list--item--secondary"><%# Eval("bpm") %> BPM</a>
														<a class="song-list--item--secondary"><%# Eval("timeSignature") %></a>
														<div class="song-list--item--icon--holder">
															<%# RenderFeatureIcon(Convert.ToBoolean(Eval("multitracks")), "ds-tracks-sm", "song-list--item--icon--wrap") %>
															<%# RenderFeatureIcon(Convert.ToBoolean(Eval("customMix")), "ds-custommix-sm", "song-list--item--icon--wrap") %>
															<%# RenderFeatureIcon(Convert.ToBoolean(Eval("rehearsalMix")), "ds-rehearsalmix-sm", "song-list--item--icon--wrap") %>
															<%# RenderFeatureIcon(Convert.ToBoolean(Eval("patches")), "ds-sounds-sm", "song-list--item--icon--wrap") %>
															<%# RenderFeatureIcon(Convert.ToBoolean(Eval("chart")), "ds-charts-sm", "song-list--item--icon--wrap") %>
															<%# RenderFeatureIcon(Convert.ToBoolean(Eval("proPresenter")), "ds-propresenter-sm", "song-list--item--icon--wrap") %>
														</div>
													</div><!-- /.song-list-item-right -->
												</li><!-- /.song-list-item -->
											</ItemTemplate>
										</asp:Repeater>
									</ul><!-- /.song-list -->
								</section><!-- /.songs-section -->

								<div class="discovery--space-saver">
									<section class="standard--holder">
										<div class="discovery--section--header">
											<h2>Albums</h2>
											<a class="discovery--section--header--view-all" href="/artists/default.aspx">View All</a>
										</div><!-- /.discovery-select -->

										<div class="discovery--grid-holder">

											<div class="ly-grid ly-grid-cranberries">
												<asp:Repeater ID="rptAlbums" runat="server">
													<ItemTemplate>
														<div class="media-item">
															<a class="media-item--img--link" href="#" tabindex="0">
																<img class="media-item--img" alt='<%# Eval("title") %>' src='<%# Eval("imageURL") %>'>
																<span class="image-tag">Master</span>
															</a>
															<a class="media-item--title" href="#" tabindex="0"><%# Eval("title") %></a>
															<a class="media-item--subtitle" href="#" tabindex="0"><%# ArtistName %></a>
														</div>
													</ItemTemplate>
												</asp:Repeater>
											</div><!-- /.grid -->
										</div><!-- /.discovery-grid-holder -->
									</section><!-- /.songs-section -->
								</div>

								<section class="standard--holder">
									<div class="discovery--section--header">
										<h2>Biography</h2>
									</div><!-- /.discovery-section-header -->

									<div class="artist-details--biography biography">
										<%=ArtistBiography%>
									</div>
								</section><!-- /.biography-section -->
							</main><!-- /.discovery-section -->
				</div><!-- /.standard-container -->
			</div><!-- /.wrapper -->

			<a class="accessibility" href="#wrapper" tabindex="20">Back to top</a>
		</form>
	</body>
</html>