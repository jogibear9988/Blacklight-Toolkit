//-----------------------------------------------------------------------
// <copyright file="LiveSearchListBox.cs" company="Microsoft Corporation copyright 2008.">
// (c) 2008 Microsoft Corporation. All rights reserved.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// </copyright>
// <date>26-Feb-2009</date>
// <author>Martin Grayson</author>
// <summary>A list box for displaying live searches.</summary>
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
    using LiveSearchService;
    using System.Globalization;
    using System.ServiceModel;
    using System.Text;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// A list box for displaying live searches.
    /// </summary>
    public class LiveSearchListBox : ListBox
    {
        /// <summary>
        /// The Text Box Name Dependency Property.
        /// </summary>
        public static readonly DependencyProperty TextBoxNameProperty =
            DependencyProperty.Register("TextBoxName", typeof(string), typeof(LiveSearchListBox), new PropertyMetadata(null, new PropertyChangedCallback(TextBoxName_Changed)));

        /// <summary>
        /// The Search Button Name Dependency Property.
        /// </summary>
        public static readonly DependencyProperty SearchButtonNameProperty =
            DependencyProperty.Register("SearchButtonName", typeof(string), typeof(LiveSearchListBox), new PropertyMetadata(null, new PropertyChangedCallback(SearchButtonName_Changed)));

        /// <summary>
        /// The First Page Button Name Dependency Property.
        /// </summary>
        public static readonly DependencyProperty FirstPageButtonNameProperty =
            DependencyProperty.Register("FirstPageButtonName", typeof(string), typeof(LiveSearchListBox), new PropertyMetadata(null, new PropertyChangedCallback(FirstPageButtonName_Changed)));

        /// <summary>
        /// The Previous Page Button Name Dependency Property.
        /// </summary>
        public static readonly DependencyProperty PreviousPageButtonNameProperty =
            DependencyProperty.Register("PreviousPageButtonName", typeof(string), typeof(LiveSearchListBox), new PropertyMetadata(null, new PropertyChangedCallback(PreviousPageButtonName_Changed)));

        /// <summary>
        /// The Next Page Button Name Dependency Property.
        /// </summary>
        public static readonly DependencyProperty NextPageButtonNameProperty =
            DependencyProperty.Register("NextPageButtonName", typeof(string), typeof(LiveSearchListBox), new PropertyMetadata(null, new PropertyChangedCallback(NextPageButtonName_Changed)));

        /// <summary>
        /// The Page Size Dependency Property.
        /// </summary>
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(LiveSearchListBox), new PropertyMetadata((int)10));

        /// <summary>
        /// The Page Offset Dependency Property.
        /// </summary>
        public static readonly DependencyProperty PageOffsetProperty =
            DependencyProperty.Register("PageOffset", typeof(int), typeof(LiveSearchListBox), new PropertyMetadata((int)0, new PropertyChangedCallback(PageProperty_Changed)));

        /// <summary>
        /// The Live Search Type Dependency Property.
        /// </summary>
        public static readonly DependencyProperty LiveSearchTypeProperty =
            DependencyProperty.Register("LiveSearchType", typeof(SourceType), typeof(LiveSearchListBox), new PropertyMetadata(SourceType.Web));

        /// <summary>
        /// The App Id Dependency Property.
        /// </summary>
        public static readonly DependencyProperty AppIdProperty =
            DependencyProperty.Register("AppId", typeof(string), typeof(LiveSearchListBox), new PropertyMetadata(string.Empty));

        /// <summary>
        /// The Endpoint Address Dependency Property.
        /// </summary>
        public static readonly DependencyProperty EndpointAddressProperty =
            DependencyProperty.Register("EndpointAddress", typeof(string), typeof(LiveSearchListBox), new PropertyMetadata("http://api.search.live.net:80/soap.asmx"));

        /// <summary>
        /// The SearchingContent Dependency Property.
        /// </summary>
        public static readonly DependencyProperty SearchingContentProperty =
            DependencyProperty.Register("SearchingContent", typeof(object), typeof(LiveSearchListBox), null);

        /// <summary>
        /// The SearchingContentTemplate Dependency Property.
        /// </summary>
        public static readonly DependencyProperty SearchingContentTemplateProperty =
            DependencyProperty.Register("SearchingContentTemplate", typeof(DataTemplate), typeof(LiveSearchListBox), null);

        /// <summary>
        /// The NoResultsContent Dependency Property.
        /// </summary>
        public static readonly DependencyProperty NoResultsContentProperty =
            DependencyProperty.Register("NoResultsContent", typeof(object), typeof(LiveSearchListBox), null);

        /// <summary>
        /// The NoResultsContentTemplate Dependency Property.
        /// </summary>
        public static readonly DependencyProperty NoResultsContentTemplateProperty =
            DependencyProperty.Register("NoResultsContentTemplate", typeof(DataTemplate), typeof(LiveSearchListBox), null);

        /// <summary>
        /// The SearchErrorContent Dependency Property.
        /// </summary>
        public static readonly DependencyProperty SearchErrorContentProperty =
            DependencyProperty.Register("SearchErrorContent", typeof(object), typeof(LiveSearchListBox), null);

        /// <summary>
        /// The SearchErrorContentTemplate Dependency Property.
        /// </summary>
        public static readonly DependencyProperty SearchErrorContentTemplateProperty =
            DependencyProperty.Register("SearchErrorContentTemplate", typeof(DataTemplate), typeof(LiveSearchListBox), null);

        /// <summary>
        /// The NoSearchContent Dependency Property.
        /// </summary>
        public static readonly DependencyProperty NoSearchContentProperty =
            DependencyProperty.Register("NoSearchContent", typeof(object), typeof(LiveSearchListBox), null);

        /// <summary>
        /// The NoSearchContentTemplate Dependency Property.
        /// </summary>
        public static readonly DependencyProperty NoSearchContentTemplateProperty =
            DependencyProperty.Register("NoSearchContentTemplate", typeof(DataTemplate), typeof(LiveSearchListBox), null);

        /// <summary>
        /// The PageButtonVisibility Dependency Property.
        /// </summary>
        public static readonly DependencyProperty PageButtonVisibilityProperty =
            DependencyProperty.Register("PageButtonVisibility", typeof(Visibility), typeof(LiveSearchListBox), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// The template part name for the searching content presenter.
        /// </summary>
        private const string ElementSearchingContentPresenter = "SearchingContentPresenter";

        /// <summary>
        /// The template part name for the no results content presenter.
        /// </summary>
        private const string ElementNoResultsContentPresenter = "NoResultsContentPresenter";

        /// <summary>
        /// The template part name for the search error content presenter.
        /// </summary>
        private const string ElementSearchErrorContentPresenter = "SearchErrorContentPresenter";

        /// <summary>
        /// The template part name for the no search content presenter.
        /// </summary>
        private const string ElementNoSearchContentPresenter = "NoSearchContentPresenter";

        /// <summary>
        /// The template part name for the first page button.
        /// </summary>
        private const string ElementFirstPageButtonName = "FirstPageButton";

        /// <summary>
        /// The template part name for the previous page button.
        /// </summary>
        private const string ElementPreviousPageButtonName = "PreviousPageButton";
        
        /// <summary>
        /// The template part name for the next page button.
        /// </summary>
        private const string ElementNextPageButtonName = "NextPageButton";

        /// <summary>
        /// Stores a linked text box.
        /// </summary>
        private TextBox textBox;

        /// <summary>
        /// Stores a linked search button.
        /// </summary>
        private Button searchButton;

        /// <summary>
        /// Stores the first page button.
        /// </summary>
        private Button firstPageButton;
        
        /// <summary>
        /// Stores the previous page button.
        /// </summary>
        private Button previousPageButton;

        /// <summary>
        /// Stores the next page button.
        /// </summary>
        private Button nextPageButton;

        /// <summary>
        /// Stores the last search.
        /// </summary>
        private string lastSearch;

        /// <summary>
        /// Stores the searching content presenter.
        /// </summary>
        private ContentPresenter searchingContentPresenter;

        /// <summary>
        /// Stores the no results content presenter.
        /// </summary>
        private ContentPresenter noResultsContentPresenter;

        /// <summary>
        /// Stores the search error content presenter.
        /// </summary>
        private ContentPresenter searchErrorContentPresenter;

        /// <summary>
        /// Stores the no search content presenter.
        /// </summary>
        private ContentPresenter noSearchContentPresenter;

        /// <summary>
        /// LiveSearchListBox constructor.
        /// </summary>
        public LiveSearchListBox()
        {
            this.DefaultStyleKey = typeof(LiveSearchListBox);
            this.Loaded += new RoutedEventHandler(this.LiveSearchListBox_Loaded);

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                this.GenerateDesignTimeData();
            }
        }

        /// <summary>
        /// The ItemsChanged Event.
        /// </summary>
        public event EventHandler ItemsChanged;

        /// <summary>
        /// Gets or sets the Text Box name.
        /// </summary>
        /// <value>The text box name.</value>
        [Category("Linked Controls"), Description("Gets or sets the Text Box name.")]
        public string TextBoxName
        {
            get 
            { 
                return (string)GetValue(TextBoxNameProperty); 
            }

            set 
            {
                if (this.TextBoxName == value)
                {
                    this.UpdateTextBox();
                }
                else
                {
                    SetValue(TextBoxNameProperty, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the search button name.
        /// </summary>
        /// <value>The search button name.</value>
        [Category("Linked Controls"), Description("Gets or sets the search button name.")]
        public string SearchButtonName
        {
            get 
            { 
                return (string)GetValue(SearchButtonNameProperty); 
            }

            set 
            {
                if (this.SearchButtonName == value)
                {
                    this.UpdateSearchButton();
                }
                else
                {
                    SetValue(SearchButtonNameProperty, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the first page button name.
        /// </summary>
        /// <value>The first page button name.</value>
        [Category("Linked Controls"), Description("Gets or sets the first page button name.")]
        public string FirstPageButtonName
        {
            get
            {
                return (string)GetValue(FirstPageButtonNameProperty);
            }

            set
            {
                if (this.FirstPageButtonName == value)
                {
                    this.UpdateFirstPageButton();
                }
                else
                {
                    SetValue(FirstPageButtonNameProperty, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the previous page button name.
        /// </summary>
        /// <value>The previous page button name.</value>
        [Category("Linked Controls"), Description("Gets or sets the previous page button name.")]
        public string PreviousPageButtonName
        {
            get
            {
                return (string)GetValue(PreviousPageButtonNameProperty);
            }

            set
            {
                if (this.PreviousPageButtonName == value)
                {
                    this.UpdatePreviousPageButton();
                }
                else
                {
                    SetValue(PreviousPageButtonNameProperty, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the next page button name.
        /// </summary>
        /// <value>The Next page button name.</value>
        [Category("Linked Controls"), Description("Gets or sets the next page button name.")]
        public string NextPageButtonName
        {
            get
            {
                return (string)GetValue(NextPageButtonNameProperty);
            }

            set
            {
                if (this.NextPageButtonName == value)
                {
                    this.UpdateNextPageButton();
                }
                else
                {
                    SetValue(NextPageButtonNameProperty, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        /// <value>The page size.</value>
        [Category("Search Properties"), Description("Gets or sets the page size.")]
        public int PageSize
        {
            get 
            { 
                return (int)GetValue(PageSizeProperty); 
            }

            set 
            {
                int pageSize = Math.Min(50, value);
                SetValue(PageSizeProperty, pageSize); 
            }
        }

        /// <summary>
        /// Gets or sets the page offset.
        /// </summary>
        /// <value>The page offset.</value>
        [Category("Search Properties"), Description("Gets or sets the page offset.")]
        public int PageOffset
        {
            get { return (int)GetValue(PageOffsetProperty); }
            set { SetValue(PageOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the live search type.
        /// </summary>
        /// <value>The live search type.</value>
        [Category("Search Properties"), Description("Gets or sets the live search type.")]
        public SourceType LiveSearchType
        {
            get { return (SourceType)GetValue(LiveSearchTypeProperty); }
            set { SetValue(LiveSearchTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the App Id.
        /// </summary>
        /// <value>The app id.</value>
        [Category("Search Properties"), Description("Gets or sets the App Id.")]
        public string AppId
        {
            get { return (string)GetValue(AppIdProperty); }
            set { SetValue(AppIdProperty, value); }
        }

        /// <summary>
        /// Gets or sets the live search end point address.
        /// </summary>
        /// <value>The live search end point address.</value>
        [Category("Search Properties"), Description("Gets or sets the live search end point address.")]
        public string EndpointAddress
        {
            get { return (string)GetValue(EndpointAddressProperty); }
            set { SetValue(EndpointAddressProperty, value); }
        }

        /// <summary>
        /// Gets or sets the searching content.
        /// </summary>
        /// <value>The searching content.</value>
        [Category("Common Properties"), Description("Gets or sets the searching content.")]
        public object SearchingContent
        {
            get { return (object)GetValue(SearchingContentProperty); }
            set { SetValue(SearchingContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the searching content template.
        /// </summary>
        /// <value>The searching content template.</value>
        [Category("Common Properties"), Description("Gets or sets the searching content template.")]
        public DataTemplate SearchingContentTemplate
        {
            get { return (DataTemplate)GetValue(SearchingContentTemplateProperty); }
            set { SetValue(SearchingContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the no results content.
        /// </summary>
        /// <value>The no results content.</value>
        [Category("Common Properties"), Description("Gets or sets the no results content.")]
        public object NoResultsContent
        {
            get { return (object)GetValue(NoResultsContentProperty); }
            set { SetValue(NoResultsContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the no results content template.
        /// </summary>
        /// <value>The no results content template.</value>
        [Category("Common Properties"), Description("Gets or sets the no results content template.")]
        public DataTemplate NoResultsContentTemplate
        {
            get { return (DataTemplate)GetValue(NoResultsContentTemplateProperty); }
            set { SetValue(NoResultsContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the search error content.
        /// </summary>
        /// <value>The search error content.</value>
        [Category("Common Properties"), Description("Gets or sets the search error content.")]
        public object SearchErrorContent
        {
            get { return (object)GetValue(SearchErrorContentProperty); }
            set { SetValue(SearchErrorContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the search error content template.
        /// </summary>
        /// <value>The search error content template.</value>
        [Category("Common Properties"), Description("Gets or sets the search error content template.")]
        public DataTemplate SearchErrorContentTemplate
        {
            get { return (DataTemplate)GetValue(SearchErrorContentTemplateProperty); }
            set { SetValue(SearchErrorContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the no search content.
        /// </summary>
        /// <value>The no search content.</value>
        [Category("Common Properties"), Description("Gets or sets the no search content.")]
        public object NoSearchContent
        {
            get { return (object)GetValue(NoSearchContentProperty); }
            set { SetValue(NoSearchContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the no search content template.
        /// </summary>
        /// <value>The no search content template.</value>
        [Category("Common Properties"), Description("Gets or sets the no search content template.")]
        public DataTemplate NoSearchContentTemplate
        {
            get { return (DataTemplate)GetValue(NoSearchContentTemplateProperty); }
            set { SetValue(NoSearchContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the page button visibility.
        /// </summary>
        /// <value>The page button visibility.</value>
        public Visibility PageButtonVisibility
        {
            get { return (Visibility)GetValue(PageButtonVisibilityProperty); }
            set { SetValue(PageButtonVisibilityProperty, value); }
        }

        /// <summary>
        /// Initiates a search.
        /// </summary>
        public void Refresh()
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                this.Search(this.lastSearch);
            }
            else
            {
                this.GenerateDesignTimeData();
            }
        }

        /// <summary>
        /// Initiates a search, with a search string.
        /// </summary>
        /// <param name="search">The search string.</param>
        public void Search(string search)
        {
            if (this.searchingContentPresenter != null)
            {
                this.searchingContentPresenter.Visibility = Visibility.Collapsed;
            }

            if (this.noResultsContentPresenter != null)
            {
                this.noResultsContentPresenter.Visibility = Visibility.Collapsed;
            }

            if (this.searchErrorContentPresenter != null)
            {
                this.searchErrorContentPresenter.Visibility = Visibility.Collapsed;
            }

            if (this.noSearchContentPresenter != null)
            {
                this.noSearchContentPresenter.Visibility = Visibility.Collapsed;
            }

            this.ItemsSource = null;
            if (!string.IsNullOrEmpty(this.AppId) && !string.IsNullOrEmpty(search))
            {
                if (this.searchingContentPresenter != null)
                {
                    this.searchingContentPresenter.Visibility = Visibility.Visible;
                }

                this.lastSearch = search;

                LiveSearchPortTypeClient client = new LiveSearchPortTypeClient(new BasicHttpBinding(), new EndpointAddress(this.EndpointAddress));
                client.SearchCompleted += new EventHandler<SearchCompletedEventArgs>(this.Client_SearchCompleted);
                SearchRequest request = new SearchRequest();

                request.AppId = this.AppId;
                request.Query = search;
                request.Sources = new SourceType[] { this.LiveSearchType };
                request.Version = "2.0";
                this.SetPageRequest(ref request);
                client.SearchAsync(request);
            }
            else if (string.IsNullOrEmpty(search))
            {
                if (this.noSearchContentPresenter != null)
                {
                    this.noSearchContentPresenter.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Moves to the first page.
        /// </summary>
        public void MovetoFirstPage()
        {
            this.PageOffset = 0;            
        }

        /// <summary>
        /// Moves the next page.
        /// </summary>
        public void MoveToNextPage()
        {
            this.PageOffset += this.PageSize;
        }

        /// <summary>
        /// Moves to the previous page.
        /// </summary>
        public void MoveToPreviousPage()
        {
            this.PageOffset = (int)Math.Max(0, (int)this.PageOffset - (int)this.PageSize);
        }

        /// <summary>
        /// Gets the template parts from the template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.searchingContentPresenter = this.GetTemplateChild(LiveSearchListBox.ElementSearchingContentPresenter) as ContentPresenter;
            if (this.searchingContentPresenter != null)
            {
                if (this.lastSearch == null)
                {
                    this.searchingContentPresenter.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.searchingContentPresenter.Visibility = Visibility.Visible;
                }
            }

            this.noResultsContentPresenter = this.GetTemplateChild(LiveSearchListBox.ElementNoResultsContentPresenter) as ContentPresenter;
            if (this.noResultsContentPresenter != null)
            {
                this.noResultsContentPresenter.Visibility = Visibility.Collapsed;
            }

            this.searchErrorContentPresenter = this.GetTemplateChild(LiveSearchListBox.ElementSearchErrorContentPresenter) as ContentPresenter;
            if (this.searchErrorContentPresenter != null)
            {
                this.searchErrorContentPresenter.Visibility = Visibility.Collapsed;
            }

            this.noSearchContentPresenter = this.GetTemplateChild(LiveSearchListBox.ElementNoSearchContentPresenter) as ContentPresenter;
            if (this.noSearchContentPresenter != null && this.lastSearch == null && !DesignerProperties.GetIsInDesignMode(this))
            {
                this.noSearchContentPresenter.Visibility = Visibility.Visible;
            }

            Button internalFirstPageButton = this.GetTemplateChild(LiveSearchListBox.ElementFirstPageButtonName) as Button;
            if (internalFirstPageButton != null)
            {
                internalFirstPageButton.Click += new RoutedEventHandler(this.FirstPageButton_Click);
            }

            Button internalPreviousPageButton = this.GetTemplateChild(LiveSearchListBox.ElementPreviousPageButtonName) as Button;
            if (internalPreviousPageButton != null)
            {
                internalPreviousPageButton.Click += new RoutedEventHandler(this.PreviousPageButton_Click);
            }

            Button internalNextPageButton = this.GetTemplateChild(LiveSearchListBox.ElementNextPageButtonName) as Button;
            if (internalNextPageButton != null)
            {
                internalNextPageButton.Click += new RoutedEventHandler(this.NextPageButton_Click);
            }
        }

        /// <summary>
        /// Updates the linked text box.
        /// </summary>
        /// <param name="dependencyObject">The LiveSearchListBox.</param>
        /// <param name="eventArgs">The Dependency Property Changed Event Args.</param>
        private static void TextBoxName_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            LiveSearchListBox liveSearchListBox = (LiveSearchListBox)dependencyObject;
            liveSearchListBox.UpdateTextBox();
        }

        /// <summary>
        /// Updates the linked search button.
        /// </summary>
        /// <param name="dependencyObject">The LiveSearchListBox.</param>
        /// <param name="eventArgs">The Dependency Property Changed Event Args.</param>
        private static void SearchButtonName_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            LiveSearchListBox liveSearchListBox = (LiveSearchListBox)dependencyObject;
            liveSearchListBox.UpdateSearchButton();
        }

        /// <summary>
        /// Updates the linked first page button.
        /// </summary>
        /// <param name="dependencyObject">The LiveSearchListBox.</param>
        /// <param name="eventArgs">The Dependency Property Changed Event Args.</param>
        private static void FirstPageButtonName_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            LiveSearchListBox liveSearchListBox = (LiveSearchListBox)dependencyObject;
            liveSearchListBox.UpdateFirstPageButton();
        }

        /// <summary>
        /// Updates the linked previous page button.
        /// </summary>
        /// <param name="dependencyObject">The LiveSearchListBox.</param>
        /// <param name="eventArgs">The Dependency Property Changed Event Args.</param>
        private static void PreviousPageButtonName_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            LiveSearchListBox liveSearchListBox = (LiveSearchListBox)dependencyObject;
            liveSearchListBox.UpdatePreviousPageButton();
        }

        /// <summary>
        /// Updates the linked next page button.
        /// </summary>
        /// <param name="dependencyObject">The LiveSearchListBox.</param>
        /// <param name="eventArgs">The Dependency Property Changed Event Args.</param>
        private static void NextPageButtonName_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            LiveSearchListBox liveSearchListBox = (LiveSearchListBox)dependencyObject;
            liveSearchListBox.UpdateNextPageButton();
        }

        /// <summary>
        /// Refresh the control.
        /// </summary>
        /// <param name="dependencyObject">The LiveSearchListBox.</param>
        /// <param name="eventArgs">The Dependency Property Changed Event Args.</param>
        private static void PageProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            LiveSearchListBox liveSearchListBox = (LiveSearchListBox)dependencyObject;
            liveSearchListBox.Refresh();
        }

        /// <summary>
        /// Sets up the search request parameters.
        /// </summary>
        /// <param name="request">The search request.</param>
        private void SetPageRequest(ref SearchRequest request)
        {
            switch (this.LiveSearchType)
            {
                case SourceType.Image:
                    request.Image = new LiveSearchService.ImageRequest();
                    request.Image.Count = (uint)this.PageSize;
                    request.Image.CountSpecified = true;
                    request.Image.Offset = (uint)this.PageOffset;
                    request.Image.OffsetSpecified = true;
                    break;
                case SourceType.News:
                    request.News = new LiveSearchService.NewsRequest();
                    this.PageSize = 10;
                    request.News.Offset = (uint)this.PageOffset;
                    request.News.OffsetSpecified = true;
                    break;
                case SourceType.Phonebook:
                    request.Phonebook = new LiveSearchService.PhonebookRequest();
                    request.Phonebook.Count = (uint)this.PageSize;
                    request.Phonebook.CountSpecified = true;
                    request.Phonebook.Offset = (uint)this.PageOffset;
                    request.Phonebook.OffsetSpecified = true;
                    break;
                case SourceType.Web:
                    request.Web = new LiveSearchService.WebRequest();
                    request.Web.Count = (uint)this.PageSize;
                    request.Web.CountSpecified = true;
                    request.Web.Offset = (uint)this.PageOffset;
                    request.Web.OffsetSpecified = true;
                    break;
            }
        }

        /// <summary>
        /// Updates the linked text box.
        /// </summary>
        private void UpdateTextBox()
        {
            if (!string.IsNullOrEmpty(this.TextBoxName))
            {
                if (this.textBox != null)
                {
                    this.textBox.KeyDown -= new KeyEventHandler(this.TextBox_KeyDown);
                }

                this.textBox = (TextBox)this.FindName(this.TextBoxName);

                if (this.textBox != null)
                {
                    this.textBox.KeyDown += new KeyEventHandler(this.TextBox_KeyDown);
                }
            }
        }

        /// <summary>
        /// Updates the linked search button.
        /// </summary>
        private void UpdateSearchButton()
        {
            if (!string.IsNullOrEmpty(this.SearchButtonName))
            {
                if (this.searchButton != null)
                {
                    this.searchButton.Click -= new RoutedEventHandler(this.SearchButton_Click);
                }

                this.searchButton = (Button)this.FindName(this.SearchButtonName);

                if (this.searchButton != null)
                {
                    this.searchButton.Click += new RoutedEventHandler(this.SearchButton_Click);
                }
            }
        }

        /// <summary>
        /// Updates the linked first page button.
        /// </summary>
        private void UpdateFirstPageButton()
        {
            if (!string.IsNullOrEmpty(this.FirstPageButtonName))
            {
                if (this.firstPageButton != null)
                {
                    this.firstPageButton.Click -= new RoutedEventHandler(this.FirstPageButton_Click);
                }

                this.firstPageButton = (Button)this.FindName(this.FirstPageButtonName);

                if (this.firstPageButton != null)
                {
                    this.firstPageButton.Click += new RoutedEventHandler(this.FirstPageButton_Click);
                }
            }
        }

        /// <summary>
        /// Updates the linked previous page button.
        /// </summary>
        private void UpdatePreviousPageButton()
        {
            if (!string.IsNullOrEmpty(this.PreviousPageButtonName))
            {
                if (this.previousPageButton != null)
                {
                    this.previousPageButton.Click -= new RoutedEventHandler(this.PreviousPageButton_Click);
                }

                this.previousPageButton = (Button)this.FindName(this.PreviousPageButtonName);

                if (this.previousPageButton != null)
                {
                    this.previousPageButton.Click += new RoutedEventHandler(this.PreviousPageButton_Click);
                }
            }
        }

        /// <summary>
        /// Updates the linked next page button.
        /// </summary>
        private void UpdateNextPageButton()
        {
            if (!string.IsNullOrEmpty(this.NextPageButtonName))
            {
                if (this.nextPageButton != null)
                {
                    this.nextPageButton.Click -= new RoutedEventHandler(this.NextPageButton_Click);
                }

                this.nextPageButton = (Button)this.FindName(this.NextPageButtonName);

                if (this.nextPageButton != null)
                {
                    this.nextPageButton.Click += new RoutedEventHandler(this.NextPageButton_Click);
                }
            }
        }

        /// <summary>
        /// Displays the search results.
        /// </summary>
        /// <param name="sender">The search client.</param>
        /// <param name="e">Search Completed Event Args.</param>
        private void Client_SearchCompleted(object sender, SearchCompletedEventArgs e)
        {
            if (this.searchingContentPresenter != null)
            {
                this.searchingContentPresenter.Visibility = Visibility.Collapsed;
            }

            if (e.Error == null && e.Result != null)
            {
                switch (this.LiveSearchType)
                {
                    case SourceType.Image:
                        if (e.Result.Image != null)
                        {
                            this.ItemsSource = e.Result.Image.Results;

                            if (e.Result.Image.Results == null && this.noResultsContentPresenter != null)
                            {
                                this.noResultsContentPresenter.Visibility = Visibility.Visible;
                            }
                        }
                        else if (this.noResultsContentPresenter != null)
                        {
                            this.noResultsContentPresenter.Visibility = Visibility.Visible;
                        }

                        break;
                    case SourceType.InstantAnswer:
                        if (e.Result.InstantAnswer != null)
                        {
                            this.ItemsSource = e.Result.InstantAnswer.Results;
                        }
                        else if (this.noResultsContentPresenter != null)
                        {
                            this.noResultsContentPresenter.Visibility = Visibility.Visible;
                        }

                        break;
                    case SourceType.News:
                        if (e.Result.News != null)
                        {
                            this.ItemsSource = e.Result.News.Results;
                        }
                        else if (this.noResultsContentPresenter != null)
                        {
                            this.noResultsContentPresenter.Visibility = Visibility.Visible;
                        }

                        break;
                    case SourceType.Phonebook:
                        if (e.Result.Phonebook != null)
                        {
                            this.ItemsSource = e.Result.Phonebook.Results;
                        }
                        else if (this.noResultsContentPresenter != null)
                        {
                            this.noResultsContentPresenter.Visibility = Visibility.Visible;
                        }

                        break;
                    case SourceType.RelatedSearch:
                        if (e.Result.RelatedSearch != null)
                        {
                            this.ItemsSource = e.Result.RelatedSearch.Results;
                        }
                        else if (this.noResultsContentPresenter != null)
                        {
                            this.noResultsContentPresenter.Visibility = Visibility.Visible;
                        }

                        break;
                    case SourceType.Spell:
                        if (e.Result.Spell != null)
                        {
                            this.ItemsSource = e.Result.Spell.Results;
                        }
                        else if (this.noResultsContentPresenter != null)
                        {
                            this.noResultsContentPresenter.Visibility = Visibility.Visible;
                        }

                        break;
                    case SourceType.Web:
                        if (e.Result.Web != null)
                        {
                            this.ItemsSource = e.Result.Web.Results;

                            if (e.Result.Web.Results == null && this.noResultsContentPresenter != null)
                            {
                                this.noResultsContentPresenter.Visibility = Visibility.Visible;
                            }
                        }

                        break;
                }

                if (this.ItemsChanged != null)
                {
                    this.ItemsChanged(this, EventArgs.Empty);
                }
            }
            else
            {
                if (e.Error != null)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    errorMessage.AppendLine("An error occured:");
                    errorMessage.AppendLine("   " + e.Error.Message);
                    errorMessage.AppendLine("       " + e.Error.StackTrace);
                    this.SearchErrorContent = errorMessage.ToString();

                    if (this.searchErrorContentPresenter != null)
                    {
                        this.searchErrorContentPresenter.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (this.noResultsContentPresenter != null)
                    {
                        this.noResultsContentPresenter.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        /// <summary>
        /// Initiates a search, from the search button.
        /// </summary>
        /// <param name="sender">The search button.</param>
        /// <param name="e">Routed Event Args.</param>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.textBox != null)
            {
                this.Search(this.textBox.Text);
            }
            else if (!string.IsNullOrEmpty(this.lastSearch))
            {
                this.Search(this.lastSearch);
            }
        }

        /// <summary>
        /// Moves to the first page of results.
        /// </summary>
        /// <param name="sender">The first page button.</param>
        /// <param name="e">Routed Event Args.</param>
        private void FirstPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.MovetoFirstPage();
        }

        /// <summary>
        /// Moves to the previous page of results.
        /// </summary>
        /// <param name="sender">The previous page button.</param>
        /// <param name="e">Routed Event Args.</param>
        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.MoveToPreviousPage();
        }

        /// <summary>
        /// Moves to the next page of results.
        /// </summary>
        /// <param name="sender">The next page button.</param>
        /// <param name="e">Routed Event Args.</param>
        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.MoveToNextPage();
        }

        /// <summary>
        /// Initiates a search when the enter key is pressed.
        /// </summary>
        /// <param name="sender">The text box.</param>
        /// <param name="e">Key Event Args.</param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Search(this.textBox.Text);
            }
        }

        /// <summary>
        /// Looks for a linked text box and search button on load.
        /// </summary>
        /// <param name="sender">The list box.</param>
        /// <param name="e">Routed Event Args.</param>
        private void LiveSearchListBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TextBoxName))
            {
                this.TextBoxName = string.Format(CultureInfo.CurrentCulture, "{0}_TextBox", this.Name);
            }
            else
            {
                this.UpdateTextBox();
            }

            if (string.IsNullOrEmpty(this.SearchButtonName))
            {
                this.SearchButtonName = string.Format(CultureInfo.CurrentCulture, "{0}_SearchButton", this.Name);
            }
            else
            {
                this.UpdateSearchButton();
            }

            if (string.IsNullOrEmpty(this.FirstPageButtonName))
            {
                this.FirstPageButtonName = string.Format(CultureInfo.CurrentCulture, "{0}_FirstPageButton", this.Name);
            }
            else
            {
                this.UpdateFirstPageButton();
            }

            if (string.IsNullOrEmpty(this.PreviousPageButtonName))
            {
                this.PreviousPageButtonName = string.Format(CultureInfo.CurrentCulture, "{0}_PreviousPageButton", this.Name);
            }
            else
            {
                this.UpdatePreviousPageButton();
            }

            if (string.IsNullOrEmpty(this.NextPageButtonName))
            {
                this.NextPageButtonName = string.Format(CultureInfo.CurrentCulture, "{0}_NextPageButton", this.Name);
            }
            else
            {
                this.UpdateNextPageButton();
            }
        }

        /// <summary>
        /// Generates some design time data.
        /// </summary>
        private void GenerateDesignTimeData()
        {
            switch (this.LiveSearchType)
            {
                case SourceType.Image:
                    break;
                case SourceType.InstantAnswer:
                    break;
                case SourceType.News:
                    break;
                case SourceType.Phonebook:
                    break;
                case SourceType.RelatedSearch:
                    break;
                case SourceType.Spell:
                    break;
                case SourceType.Web:
                    break;
            }

            List<WebResult> results = new List<WebResult>();
            for (int i = 0; i < this.PageSize; i++)
            {
                results.Add(new WebResult()
                {
                    DateTime = DateTime.Now.ToString(),
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean nec magna tempus nulla ornare sollicitudin. Proin massa nunc.",
                    DisplayUrl = "http://www.codeplex.com/blacklight",
                    Title = "Lorem ipsum dolor sit amet, consectetur cras amet. ",
                    Url = "http://www.codeplex.com/blacklight",
                    SearchTags = new WebSearchTag[]
                            {
                                new WebSearchTag()
                                {
                                    Name = "Web Search Tag 1",
                                    Value = "Web Search Tag 1"
                                },
                                new WebSearchTag()
                                {
                                    Name = "Web Search Tag 2",
                                    Value = "Web Search Tag 2"
                                },
                                new WebSearchTag()
                                {
                                    Name = "Web Search Tag 3",
                                    Value = "Web Search Tag 3"
                                },
                            }
                });
            }

            this.ItemsSource = results;
        }
    }
}
