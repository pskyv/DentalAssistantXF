using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DentalAssistantXF.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardLayout : Frame
    {
        public CardLayout()
        {
            InitializeComponent();
            //imgIcon.Source = Icon;
            //lblTitle.Text = Title;
            //lblTitle.Style = TitleStyle;
            //contentView.Content = CardContent;
            //actionView.Content = ActionView;
        }
        #region Bindable Properties

        public static BindableProperty VLineColorProperty = BindableProperty.Create(nameof(VLineColor), typeof(Color), typeof(CardLayout), Color.FromHex("00BCD4"), BindingMode.TwoWay, null, propertyChanged: OnLineColorPropertyChanged);        

        public Color VLineColor
        {
            get { return (Color)GetValue(VLineColorProperty); }
            set { SetValue(VLineColorProperty, value); }
        }

        public static BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create(nameof(HeaderBackgroundColor), typeof(Color), typeof(CardLayout), Color.FromHex("eeeeee"), BindingMode.TwoWay, null, propertyChanged: OnHeaderColorPropertyChanged);        

        public Color HeaderBackgroundColor
        {
            get { return (Color)GetValue(HeaderBackgroundColorProperty); }
            set { SetValue(HeaderBackgroundColorProperty, value); }
        }

        #region Icon

        public static BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(CardLayout), propertyChanged: IconTitleGridVisible);

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        #endregion

        #region Title

        public static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(CardLayout), propertyChanged: IconTitleGridVisible);

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        #endregion

        #region TitleStyle

        public static BindableProperty TitleStyleProperty = BindableProperty.Create(nameof(TitleStyle), typeof(Style), typeof(CardLayout));

        public Style TitleStyle
        {
            get { return (Style)GetValue(TitleStyleProperty); }
            set { SetValue(TitleStyleProperty, value); }
        }

        #endregion

        #region Main Content

        public static BindableProperty CardContentProperty = BindableProperty.Create(nameof(CardContent), typeof(View), typeof(CardLayout), propertyChanged: IconTitleGridVisible);

        public View CardContent
        {
            get { return (View)GetValue(CardContentProperty); }
            set { SetValue(CardContentProperty, value); }
        }

        #endregion

        #region Action Items View

        public static BindableProperty ActionViewProperty = BindableProperty.Create(nameof(ActionView), typeof(View), typeof(CardLayout));

        public View ActionView
        {
            get { return (View)GetValue(ActionViewProperty); }
            set { SetValue(ActionViewProperty, value); }
        }

        #endregion

        #endregion


        #region Property Changed Methods

        private static void OnLineColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CardLayout)bindable).VLineColor = (Color)newValue;
        }

        private static void OnHeaderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CardLayout)bindable).HeaderBackgroundColor = (Color)newValue;
        }

        private static void IconTitleGridVisible(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CardLayout)bindable;

            view.HeaderContainer.IsVisible = (!string.IsNullOrEmpty(view.Title) || view.Icon != null);
        }

        #endregion
    }
}