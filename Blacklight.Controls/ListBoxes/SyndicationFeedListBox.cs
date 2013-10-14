//-----------------------------------------------------------------------
// <copyright file="SyndicationFeedListBox.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>26-Feb-2009</date>
// <author>Martin Grayson</author>
// <summary>A list box for displaying feeds.</summary>
//-----------------------------------------------------------------------
namespace Blacklight.Controls
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using System.ServiceModel.Syndication;
    using System.Xml;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Browser;
    using System.Globalization;
    using System.Text;
    using System.ComponentModel;

    /// <summary>
    /// A list box for displaying feeds.
    /// </summary>
    public class SyndicationFeedListBox : ListBox
    {
        /// <summary>
        /// The Feed Uri Dependency Property.
        /// </summary>
        public static readonly DependencyProperty FeedUriProperty =
           DependencyProperty.Register("FeedUri", typeof(string), typeof(SyndicationFeedListBox), new PropertyMetadata(string.Empty, new PropertyChangedCallback(FeedUri_Changed)));

        /// <summary>
        /// The Feed Uri Kind Dependency Property.
        /// </summary>
        public static readonly DependencyProperty FeedUriKindProperty =
            DependencyProperty.Register("FeedUriKind", typeof(UriKind), typeof(SyndicationFeedListBox), new PropertyMetadata(UriKind.RelativeOrAbsolute));

        /// <summary>
        /// The Refresh Button Name Dependency Property.
        /// </summary>
        public static readonly DependencyProperty RefreshButtonNameProperty =
            DependencyProperty.Register("RefreshButtonName", typeof(string), typeof(SyndicationFeedListBox), new PropertyMetadata(null, new PropertyChangedCallback(RefreshButtonName_Changed)));

        /// <summary>
        /// The RefreshingContent Dependency Property.
        /// </summary>
        public static readonly DependencyProperty RefreshingContentProperty =
            DependencyProperty.Register("RefreshingContent", typeof(object), typeof(SyndicationFeedListBox), null);

        /// <summary>
        /// The RefreshingContentTemplate Dependency Property.
        /// </summary>
        public static readonly DependencyProperty RefreshingContentTemplateProperty =
            DependencyProperty.Register("RefreshingContentTemplate", typeof(DataTemplate), typeof(SyndicationFeedListBox), null);

        /// <summary>
        /// The FeedErrorContent Dependency Property.
        /// </summary>
        public static readonly DependencyProperty FeedErrorContentProperty =
            DependencyProperty.Register("FeedErrorContent", typeof(object), typeof(SyndicationFeedListBox), null);

        /// <summary>
        /// The FeedErrorContentTemplate Dependency Property.
        /// </summary>
        public static readonly DependencyProperty FeedErrorContentTemplateProperty =
            DependencyProperty.Register("FeedErrorContentTemplate", typeof(DataTemplate), typeof(SyndicationFeedListBox), null);

        /// <summary>
        /// The template part name for the searching content presenter.
        /// </summary>
        private const string ElementRefreshingContentPresenter = "RefreshingContentPresenter";

        /// <summary>
        /// The template part name for the search error content presenter.
        /// </summary>
        private const string ElementFeedErrorContentPresenter = "FeedErrorContentPresenter";

        /// <summary>
        /// Stores the refresh button.
        /// </summary>
        private Button refreshButton;

        /// <summary>
        /// Stores the refreshing content presenter.
        /// </summary>
        private ContentPresenter refreshingContentPresenter;

        /// <summary>
        /// Stores the search error content presenter.
        /// </summary>
        private ContentPresenter feedErrorContentPresenter;

        /// <summary>
        /// Gets or sets whether the control has loaded.
        /// </summary>
        private bool loaded;

        /// <summary>
        /// SyndicationFeedItemsControl constructor.
        /// </summary>
        public SyndicationFeedListBox()
        {
            this.DefaultStyleKey = typeof(SyndicationFeedListBox);
            this.Loaded += new RoutedEventHandler(this.SyndicationFeedItemsControl_Loaded);
        }

        /// <summary>
        /// Gets or sets the feed Uri.
        /// </summary>
        /// <value>The feed Uri.</value>
        [Category("Feed Properties"), Description("Gets or sets the feed Uri.")]
        public string FeedUri
        {
            get { return (string)GetValue(FeedUriProperty); }
            set { SetValue(FeedUriProperty, value); }
        }

        /// <summary>
        /// Gets or sets the feed Uri kind.
        /// </summary>
        /// <value>The feed uri kind.</value>
        [Category("Feed Properties"), Description("Gets or sets the feed Uri kind.")]
        public UriKind FeedUriKind
        {
            get { return (UriKind)GetValue(FeedUriKindProperty); }
            set { SetValue(FeedUriKindProperty, value); }
        }

        /// <summary>
        /// Gets or sets the refreshing content.
        /// </summary>
        /// <value>The refreshing content.</value>
        [Category("Common Properties"), Description("Gets or sets the refreshing content.")]
        public object RefreshingContent
        {
            get { return (object)GetValue(RefreshingContentProperty); }
            set { SetValue(RefreshingContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the refreshing content template.
        /// </summary>
        /// <value>The refreshing content template.</value>
        [Category("Common Properties"), Description("Gets or sets the refreshing content template.")]
        public DataTemplate RefreshingContentTemplate
        {
            get { return (DataTemplate)GetValue(RefreshingContentTemplateProperty); }
            set { SetValue(RefreshingContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the feed error content.
        /// </summary>
        /// <value>The feed error content.</value>
        [Category("Common Properties"), Description("Gets or sets the feed error content.")]
        public object FeedErrorContent
        {
            get { return (object)GetValue(FeedErrorContentProperty); }
            set { SetValue(FeedErrorContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the feed error content template.
        /// </summary>
        /// <value>The feed error content template.</value>
        [Category("Common Properties"), Description("Gets or sets the feed error content template.")]
        public DataTemplate FeedErrorContentTemplate
        {
            get { return (DataTemplate)GetValue(FeedErrorContentTemplateProperty); }
            set { SetValue(FeedErrorContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the refresh button name.
        /// </summary>
        /// <value>The refresh button name.</value>
        [Category("Linked Controls"), Description("Gets or sets the refresh button name.")]
        public string RefreshButtonName
        {
            get 
            { 
                return (string)GetValue(RefreshButtonNameProperty); 
            }

            set 
            {
                if (this.RefreshButtonName == value)
                {
                    this.UpdateRefreshButton();
                }
                else
                {
                    SetValue(RefreshButtonNameProperty, value);
                }
            }
        }

        /// <summary>
        /// Refreshes the feed.
        /// </summary>
        public void Refresh()
        {
            if (this.loaded && !System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                Uri uri = null;
                this.ItemsSource = null;
                if (!string.IsNullOrEmpty(this.FeedUri) && Uri.TryCreate(this.FeedUri, this.FeedUriKind, out uri))
                {
                    switch (this.FeedUriKind)
                    {
                        case UriKind.Relative:
                            this.GetFeed(new Uri(HtmlPage.Document.DocumentUri.ToString().Replace(HtmlPage.Document.DocumentUri.AbsolutePath, "") + "/" + this.FeedUri));
                            break;
                        default:
                            this.GetFeed(new Uri(this.FeedUri, this.FeedUriKind));
                            break;
                    }
                }
            }
            else if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                string designTimeXml = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><rss version=\"2.0\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:slash=\"http://purl.org/rss/1.0/modules/slash/\" xmlns:wfw=\"http://wellformedweb.org/CommentAPI/\" xmlns:media=\"http://search.yahoo.com/mrss/\" xmlns:evnet=\"http://www.mscommunities.com/rssmodule/\"><channel><title>Archived News</title><link>http://silverlight.net/blogs/news/default.aspx</link><description /><dc:language>en</dc:language><generator>CommunityServer 2007 (Build: 20416.853)</generator><item><title>New Silverlight Toolkit Video</title><link>http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls</link><pubDate>Fri, 27 Feb 2009 01:45:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:180093</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>Jesse Liberty demonstrates how to &lt;A class=\"\" href=\"http://silverlight.net/learn/learnvideo.aspx?video=180072\" mce_href=\"http://silverlight.net/learn/learnvideo.aspx?video=180072\"&gt;use the wrapper class to add controls to a panel&lt;/A&gt;, and more. Watch this and other Silverlight videos on the &lt;A class=\"\" href=\"http://silverlight.net/Learn/\" mce_href=\"http://silverlight.net/Learn/\"&gt;Learn&lt;/A&gt; page.&lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=180093\" width=\"1\" height=\"1\"&gt;</description><evnet:views>715</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=180093</evnet:viewtrackingurl></item><item><title>12 New Silverlight Showcase Entries</title><link>http://silverlight.net/showcase/</link><pubDate>Thu, 26 Feb 2009 00:15:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:179403</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>Organize your calendar with Scheduler for Silverlight, play ChessBin Chess, see the City of Boise Strategic Plan In Action, or try the Rootbeer Maze, and more in the &lt;A class=\"\" href=\"http://silverlight.net/Showcase/\" mce_href=\"http://silverlight.net/Showcase/\"&gt;Silverlight Showcase&lt;/A&gt;. &lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=179403\" width=\"1\" height=\"1\"&gt;</description><evnet:views>790</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=179403</evnet:viewtrackingurl></item><item><title>Seven New Community Gallery Entries</title><link>http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2</link><pubDate>Wed, 25 Feb 2009 01:55:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:178896</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>Play the &lt;A class=\"\" href=\"http://silverlight.net/community/gallerydetail.aspx?cat=sl2&amp;amp;sort=1#vid2521\" mce_href=\"http://silverlight.net/community/gallerydetail.aspx?cat=sl2&amp;amp;sort=1#vid2521\"&gt;Silverlight n-Puzzle Game&lt;/A&gt;, find your favorite music using &lt;A class=\"\" href=\"http://silverlight.net/community/gallerydetail.aspx?cat=sl2&amp;amp;sort=1#vid2513\" mce_href=\"http://silverlight.net/community/gallerydetail.aspx?cat=sl2&amp;amp;sort=1#vid2513\"&gt;ComponentOne SilverTunes&lt;/A&gt;, score at the &lt;A class=\"\" href=\"http://silverlight.net/community/gallerydetail.aspx?cat=sl2&amp;amp;sort=1#vid2523\" mce_href=\"http://silverlight.net/community/gallerydetail.aspx?cat=sl2&amp;amp;sort=1#vid2523\"&gt;Turkish League Futbol Game&lt;/A&gt;, and more! Find inspiration and &lt;A class=\"\" href=\"http://silverlight.net/community/galleryupload.aspx\" mce_href=\"http://silverlight.net/community/galleryupload.aspx\"&gt;upload&lt;/A&gt; your Silverlight projects to share with the community in the &lt;A class=\"\" href=\"http://silverlight.net/community/communitygallery.aspx\" mce_href=\"http://silverlight.net/community/communitygallery.aspx\"&gt;Gallery&lt;/A&gt;. &lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=178896\" width=\"1\" height=\"1\"&gt;</description><evnet:views>849</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=178896</evnet:viewtrackingurl></item><item><title>Two New Videos on Creating a Carousel in Silverlight</title><link>http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls</link><pubDate>Thu, 19 Feb 2009 23:49:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:176733</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>In &lt;A class=\"\" href=\"http://silverlight.net/learn/learnvideo.aspx?video=176700\" mce_href=\"http://silverlight.net/learn/learnvideo.aspx?video=176700\"&gt;Part 1&lt;/A&gt; of this video series, Jesse Liberty shows how to create a carousel in Silverlight, and in &lt;A class=\"\" href=\"http://silverlight.net/learn/learnvideo.aspx?video=176709\" mce_href=\"http://silverlight.net/learn/learnvideo.aspx?video=176709\"&gt;Part 2&lt;/A&gt; he explores advanced topics such as hand-tooling animation by overriding the ArrangeOverride and MeasureOverride methods, and more. &lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=176733\" width=\"1\" height=\"1\"&gt;</description><evnet:views>1379</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=176733</evnet:viewtrackingurl></item><item><title>15 New Silverlight Showcase Entries</title><link>http://silverlight.net/Showcase/</link><pubDate>Wed, 18 Feb 2009 00:49:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:175632</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>Take the Periodic Table of Elements Quiz, see if you can stay afloat in the Tire Storm, draw online with friends using Multiplayer Paint, see how much power can be generated with the Wind Energy Simulator, and more in the &lt;A class=\"\" href=\"http://silverlight.net/Showcase/\" mce_href=\"http://silverlight.net/Showcase/\"&gt;Silverlight Showcase&lt;/A&gt;. &lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=175632\" width=\"1\" height=\"1\"&gt;</description><evnet:views>1741</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=175632</evnet:viewtrackingurl></item><item><title>New Video on Displaying and Manipulating Data</title><link>http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Data</link><pubDate>Thu, 12 Feb 2009 21:10:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:173679</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>In Part 1 of this video series, Jesse Liberty sets up the infrastructure he’ll be using in future videos and &lt;A class=\"\" href=\"http://silverlight.net/learn/learnvideo.aspx?video=172574\" mce_href=\"http://silverlight.net/learn/learnvideo.aspx?video=172574\"&gt;explores various alternatives for retrieving, transporting, and displaying data in Silverlight&lt;/A&gt;. Watch this and other Silverlight videos on the &lt;A class=\"\" href=\"http://silverlight.net/Learn/\" mce_href=\"http://silverlight.net/Learn/\"&gt;Learn&lt;/A&gt; page. &lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=173679\" width=\"1\" height=\"1\"&gt;</description><evnet:views>1760</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=173679</evnet:viewtrackingurl></item><item><title>8 New Silverlight Showcase Entries</title><link>http://silverlight.net/showcase/</link><pubDate>Wed, 11 Feb 2009 00:18:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:172538</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>Crack an egg in the Golden Egg Smashing Game, test your flying skills with LaserCopter Inferno II, discover Quince - UX Patterns Explorer, play a Sudoku Puzzle, and more in the &lt;A class=\"\" href=\"http://silverlight.net/showcase/\" mce_href=\"http://silverlight.net/showcase/\"&gt;Silverlight Showcase&lt;/A&gt;. &lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=172538\" width=\"1\" height=\"1\"&gt;</description><evnet:views>1628</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=172538</evnet:viewtrackingurl></item><item><title>New Community Gallery Entry</title><link>http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2</link><pubDate>Wed, 11 Feb 2009 00:14:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:172540</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>Try the &lt;A class=\"\" href=\"http://silverlight.net/community/gallerydetail.aspx?cat=sl2&amp;amp;sort=1#vid2498\" mce_href=\"http://silverlight.net/community/gallerydetail.aspx?cat=sl2&amp;amp;sort=1#vid2498\"&gt;Silverlight Metronome&lt;/A&gt;, a classical metronome built in Silverlight. Find inspiration and &lt;A class=\"\" href=\"http://silverlight.net/community/galleryupload.aspx\" mce_href=\"http://silverlight.net/community/galleryupload.aspx\"&gt;upload&lt;/A&gt; your Silverlight projects to share with the community in the &lt;A class=\"\" href=\"http://silverlight.net/community/communitygallery.aspx\" mce_href=\"http://silverlight.net/community/communitygallery.aspx\"&gt;Gallery&lt;/A&gt;.&lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=172540\" width=\"1\" height=\"1\"&gt;</description><evnet:views>1556</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=172540</evnet:viewtrackingurl></item><item><title>New Silverlight DataGrid Video</title><link>http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Data</link><pubDate>Fri, 06 Feb 2009 19:23:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:170747</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>Todd Miranda demonstrates how to create DataGrid &lt;A class=\"\" href=\"http://silverlight.net/learn/learnvideo.aspx?video=170678\" mce_href=\"http://silverlight.net/learn/learnvideo.aspx?video=170678\"&gt;columns dynamically at runtime&lt;/A&gt;. Watch this and other Silverlight videos on the &lt;A class=\"\" href=\"http://silverlight.net/Learn/\" mce_href=\"http://silverlight.net/Learn/\"&gt;Learn&lt;/A&gt; page.&lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=170747\" width=\"1\" height=\"1\"&gt;</description><evnet:views>1948</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=170747</evnet:viewtrackingurl></item><item><title>New Silverlight Tutorial on DataEntities and WCF Feeding</title><link>http://silverlight.net/learn/tutorials/adonetdataentities_vb.aspx</link><pubDate>Thu, 05 Feb 2009 23:21:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:170722</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>In the newest addition to &lt;EM&gt;The Silverlight Geek Tutorials&lt;/EM&gt;, Jesse Liberty shows how to move data from a database to the User Interface. See this and other &lt;A class=\"\" href=\"http://silverlight.net/learn/tutorials.aspx\" mce_href=\"http://silverlight.net/learn/tutorials.aspx\"&gt;Silverlight 2 tutorials&lt;/A&gt; on the &lt;A class=\"\" href=\"http://silverlight.net/Learn/\" mce_href=\"http://silverlight.net/Learn/\"&gt;Learn&lt;/A&gt; page.&lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=170722\" width=\"1\" height=\"1\"&gt;</description><evnet:views>2071</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=170722</evnet:viewtrackingurl></item><item><title>32 New Silverlight Showcase Entries</title><link>http://silverlight.net/Showcase/</link><pubDate>Thu, 05 Feb 2009 01:52:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:170298</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>Find your way around campus with iMap – Campus Maps, scale a mountain with the Expedition Korumdy 2008 team, try your hand at Speed Poker, or enjoy a game of Mahjong Solitaire, and more in the &lt;A class=\"\" href=\"http://silverlight.net/Showcase/\" mce_href=\"http://silverlight.net/Showcase/\"&gt;Silverlight Showcase&lt;/A&gt;. &lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=170298\" width=\"1\" height=\"1\"&gt;</description><evnet:views>1628</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=170298</evnet:viewtrackingurl></item><item><title>Two New Silverlight Data Videos</title><link>http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Data</link><pubDate>Fri, 30 Jan 2009 00:03:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:167779</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>Todd Miranda demonstrates how to use &lt;A class=\"\" href=\"http://silverlight.net/learn/learnvideo.aspx?video=167329\" mce_href=\"http://silverlight.net/learn/learnvideo.aspx?video=167329\"&gt;frozen columns in the Silverlight DataGrid&lt;/A&gt;.&amp;nbsp; Mik Chernomordikov shows how to &lt;A class=\"\" href=\"http://silverlight.net/learn/learnvideo.aspx?video=167357\" mce_href=\"http://silverlight.net/learn/learnvideo.aspx?video=167357\"&gt;create a Mesh-enabled Web Application with Silverlight&lt;/A&gt;. &lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=167779\" width=\"1\" height=\"1\"&gt;</description><evnet:views>2079</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=167779</evnet:viewtrackingurl></item><item><title>Seven New Community Gallery Entries</title><link>http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2</link><pubDate>Wed, 28 Jan 2009 00:56:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:166826</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>Try to win at &lt;A class=\"\" href=\"http://silverlight.net/community/gallerydetail.aspx?cat=sl2&amp;amp;sort=1#vid2442\" mce_href=\"http://silverlight.net/community/gallerydetail.aspx?cat=sl2&amp;amp;sort=1#vid2442\"&gt;Tic Tac Toe&lt;/A&gt;, experiment with the &lt;A class=\"\" href=\"http://silverlight.net/community/gallerydetail.aspx?cat=sl2&amp;amp;sort=1#vid2420\" mce_href=\"http://silverlight.net/community/gallerydetail.aspx?cat=sl2&amp;amp;sort=1#vid2420\"&gt;Dynamic Loading Media Player&lt;/A&gt;, play a &lt;A class=\"\" href=\"http://silverlight.net/community/gallerydetail.aspx?cat=sl2&amp;amp;sort=1#vid2412\" mce_href=\"http://silverlight.net/community/gallerydetail.aspx?cat=sl2&amp;amp;sort=1#vid2412\"&gt;Simple VB Game&lt;/A&gt;, and more! Find inspiration and &lt;A class=\"\" href=\"http://silverlight.net/community/galleryupload.aspx\" mce_href=\"http://silverlight.net/community/galleryupload.aspx\"&gt;upload&lt;/A&gt; your Silverlight projects to share with the community in the &lt;A class=\"\" href=\"http://silverlight.net/community/communitygallery.aspx\" mce_href=\"http://silverlight.net/community/communitygallery.aspx\"&gt;Gallery&lt;/A&gt;. &lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=166826\" width=\"1\" height=\"1\"&gt;</description><evnet:views>2355</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=166826</evnet:viewtrackingurl></item><item><title>New Silverlight Toolkit and Media Videos</title><link>http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls</link><pubDate>Thu, 22 Jan 2009 23:57:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:164851</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>Jesse Liberty demonstrates how to &lt;A class=\"\" href=\"http://silverlight.net/learn/learnvideo.aspx?video=164686\" mce_href=\"http://silverlight.net/learn/learnvideo.aspx?video=164686\"&gt;create and use the HeaderContentControl and the HeaderItemsControl&lt;/A&gt; from the Silverlight Toolkit and he concludes his three part Hypervideo series with &lt;A class=\"\" href=\"http://silverlight.net/learn/learnvideo.aspx?video=164685\" mce_href=\"http://silverlight.net/learn/learnvideo.aspx?video=164685\"&gt;Hypervideo, Part 3&lt;/A&gt;. &lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=164851\" width=\"1\" height=\"1\"&gt;</description><evnet:views>2562</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=164851</evnet:viewtrackingurl></item><item><title>32 New Silverlight Showcase Entries</title><link>http://silverlight.net/Showcase/</link><pubDate>Wed, 21 Jan 2009 01:08:00 GMT</pubDate><guid isPermaLink=\"false\">d0d632c8-a6f7-4f68-b0ce-26aaafd62132:163753</guid><dc:creator>christinemckinnon</dc:creator><slash:comments>0</slash:comments><description>Find vintage collectibles with the Era Browser, listen to classical music at Plastide, share your favorite images at Imaginalaxy, let CalSharp evaluate your mathematical expressions, and more in the &lt;A class=\"\" href=\"http://silverlight.net/Showcase/\" mce_href=\"http://silverlight.net/Showcase/\"&gt;Silverlight Showcase.&lt;/A&gt; &lt;img src=\"http://silverlight.net/aggbug.aspx?PostID=163753\" width=\"1\" height=\"1\"&gt;</description><evnet:views>2603</evnet:views><evnet:viewtrackingurl>http://silverlight.net/aggbug.aspx?PostID=163753</evnet:viewtrackingurl></item></channel></rss>";
                XmlReader xmlReader = XmlReader.Create(new StringReader(designTimeXml));
                SyndicationFeed feed = SyndicationFeed.Load(xmlReader);
                this.ItemsSource = feed.Items;
            }
        }

        /// <summary>
        /// Gets the parts from the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.refreshingContentPresenter = this.GetTemplateChild(SyndicationFeedListBox.ElementRefreshingContentPresenter) as ContentPresenter;
            if (this.refreshingContentPresenter != null)
            {
                this.refreshingContentPresenter.Visibility = Visibility.Collapsed;
            }

            this.feedErrorContentPresenter = this.GetTemplateChild(SyndicationFeedListBox.ElementFeedErrorContentPresenter) as ContentPresenter;
            if (this.feedErrorContentPresenter != null)
            {
                this.feedErrorContentPresenter.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Refreshes the feed on a property change.
        /// </summary>
        /// <param name="dependencyObject">The syndication feed items control.</param>
        /// <param name="eventArgs">Dependency Property Changed Event Args.</param>
        private static void FeedUri_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            SyndicationFeedListBox syndicationFeedItemsControl = (SyndicationFeedListBox)dependencyObject;
            syndicationFeedItemsControl.Refresh();
        }

        /// <summary>
        /// Updates the linked refresh button.
        /// </summary>
        /// <param name="dependencyObject">The LiveSearchListBox.</param>
        /// <param name="eventArgs">The Dependency Property Changed Event Args.</param>
        private static void RefreshButtonName_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            SyndicationFeedListBox syndicationFeedItemsControl = (SyndicationFeedListBox)dependencyObject;
            syndicationFeedItemsControl.UpdateRefreshButton();
        }

        /// <summary>
        /// Updates the linked refresh button.
        /// </summary>
        private void UpdateRefreshButton()
        {
            if (this.refreshButton != null)
            {
                this.refreshButton.Click -= new RoutedEventHandler(this.RefreshButton_Click);
            }

            this.refreshButton = (Button)this.FindName(this.RefreshButtonName);

            if (this.refreshButton != null)
            {
                this.refreshButton.Click += new RoutedEventHandler(this.RefreshButton_Click);
            }
        }

        /// <summary>
        /// Gets a feed from a Uri.
        /// </summary>
        /// <param name="uri">The Uri of the feed.</param>
        private void GetFeed(Uri uri)
        {
            if (this.feedErrorContentPresenter != null)
            {
                this.feedErrorContentPresenter.Visibility = Visibility.Collapsed;
            }

            if (this.refreshingContentPresenter != null)
            {
                this.refreshingContentPresenter.Visibility = Visibility.Visible;
            }

            WebClient client = new WebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(this.Client_OpenReadCompleted);
            client.OpenReadAsync(uri);
        }

        /// <summary>
        /// Gets the response, and parses as a feed.
        /// </summary>
        /// <param name="sender">The web client.</param>
        /// <param name="e">Open Read Completed Event Args</param>
        private void Client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (this.refreshingContentPresenter != null)
            {
                this.refreshingContentPresenter.Visibility = Visibility.Collapsed;
            }

            if (e.Error == null && e.Result != null)
            {
                try
                {
                    XmlReader xmlReader = XmlReader.Create(e.Result);
                    SyndicationFeed feed = SyndicationFeed.Load(xmlReader);
                    this.ItemsSource = feed.Items;

                    if (this.feedErrorContentPresenter != null)
                    {
                        this.feedErrorContentPresenter.Visibility = Visibility.Collapsed;
                    }
                }
                catch (XmlException exception)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    errorMessage.AppendLine("An error occured:");
                    errorMessage.AppendLine("   " + exception.Message);
                    errorMessage.AppendLine("       " + exception.StackTrace);
                    this.FeedErrorContent = errorMessage.ToString();

                    if (this.feedErrorContentPresenter != null)
                    {
                        this.feedErrorContentPresenter.Visibility = Visibility.Visible;
                    }
                }
            }
            else if (e.Error != null)
            {
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.AppendLine("An error occured:");
                errorMessage.AppendLine("   " + e.Error.Message);
                errorMessage.AppendLine("       " + e.Error.StackTrace);
                this.FeedErrorContent = errorMessage.ToString();

                if (this.feedErrorContentPresenter != null)
                {
                    this.feedErrorContentPresenter.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Refresh the feed.
        /// </summary>
        /// <param name="sender">The refresh button.</param>
        /// <param name="e">Routed Event Args.</param>
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            this.Refresh();
        }

        /// <summary>
        /// Updates the linked UI elements.
        /// </summary>
        /// <param name="sender">The syndication feed items control.</param>
        /// <param name="e">Routed EventArgs.</param>
        private void SyndicationFeedItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.loaded = true;
            this.Refresh();
            if (string.IsNullOrEmpty(this.RefreshButtonName))
            {
                this.RefreshButtonName = string.Format(CultureInfo.CurrentCulture, "{0}_RefreshButton", this.Name);
            }
            else
            {
                this.UpdateRefreshButton();
            }
        }
    }
}
