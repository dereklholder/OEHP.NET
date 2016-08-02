using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Web;
using System.Collections.Specialized;

namespace OEHP_Tester
{
    /// <summary>
    /// Interaction logic for PayPageBranding.xaml
    /// </summary>
    public partial class PayPageBranding : Window
    {
        public PayPageBranding()
        {
            InitializeComponent();
            if (Globals.Default.CustomParameters != "")
            {
                ParseQueryStringFromCustomParameters(Globals.Default.CustomParameters);
            }
            
        }
        public void ParseQueryStringFromCustomParameters(string parameters)
        {
            
            if (parameters != "")
            {
                NameValueCollection keyPairs = HttpUtility.ParseQueryString(parameters);

                PayPageCosmetic.Default.font_family = keyPairs.Get("font-family").Trim(';');
                PayPageCosmetic.Default.font_size = keyPairs.Get("font-size").Trim(';');
                PayPageCosmetic.Default.color = keyPairs.Get("color").Trim(';');
                PayPageCosmetic.Default.background_color = keyPairs.Get("background-color").Trim(';');
                PayPageCosmetic.Default.btn_color = keyPairs.Get("btn_color").Trim(';');
                PayPageCosmetic.Default.btn_background_color = keyPairs.Get("btn-background-color").Trim(';');
                PayPageCosmetic.Default.btn_height = keyPairs.Get("btn-height").Trim(';');
                PayPageCosmetic.Default.btn_width = keyPairs.Get("btn-width").Trim(';');
                PayPageCosmetic.Default.btn_border_top_left_radius = keyPairs.Get("btn-border-top-left-radius").Trim(';');
                PayPageCosmetic.Default.btn_border_top_right_radius = keyPairs.Get("btn-border-top-right-radius").Trim(';');
                PayPageCosmetic.Default.btn_border_bottom_left_radius = keyPairs.Get("btn-border-bottom-left-radius").Trim(';');
                PayPageCosmetic.Default.btn_border_bottom_right_radius = keyPairs.Get("btn-border-bottom-right-radius").Trim(';');
                PayPageCosmetic.Default.btn_border_color = keyPairs.Get("btn-border-color").Trim(';');
                PayPageCosmetic.Default.btn_border_style = keyPairs.Get("btn-border-style").Trim(';');
                PayPageCosmetic.Default.section_header_font_size = keyPairs.Get("section-header-font-size").Trim(';');
                PayPageCosmetic.Default.line_spacing_size = keyPairs.Get("line-spacing-size").Trim(';');
                PayPageCosmetic.Default.input_field_height = keyPairs.Get("input-field-height").Trim(';');

                PayPageCosmetic.Default.Save(); 
            }
        }
        public string PayPageBrandingCombiner()
        {
            StringBuilder sb = new StringBuilder();
            if (PayPageCosmetic.Default.font_family != "")
            {
                sb.Append("&font-family=")
                  .Append(PayPageCosmetic.Default.font_family)
                  .Append(';');
            }
            if (PayPageCosmetic.Default.font_size != "")
            {
                sb.Append("&font-size=")
                  .Append(PayPageCosmetic.Default.font_size)
                  .Append(';');
            }
            if (PayPageCosmetic.Default.color != "")
            {
                sb.Append("&color=")
                   .Append(PayPageCosmetic.Default.color)
                   .Append(';');
            }
            if (PayPageCosmetic.Default.background_color != "")
            {
                sb.Append("&background-color=")
                    .Append(PayPageCosmetic.Default.background_color)
                    .Append(';');
            }
            if (PayPageCosmetic.Default.btn_color != "")
            {
                sb.Append("&btn-color=")
                    .Append(PayPageCosmetic.Default.btn_color)
                    .Append(';');
            }
            if (PayPageCosmetic.Default.btn_background_color != "")
            {
                sb.Append("&btn-background-color=")
                    .Append(PayPageCosmetic.Default.btn_background_color)
                    .Append(';');
            }
            if (PayPageCosmetic.Default.btn_height != "")
            {
                sb.Append("&btn-height=")
                    .Append(PayPageCosmetic.Default.btn_height)
                    .Append(';');
            }
            if (PayPageCosmetic.Default.btn_width != "")
            {
                sb.Append("&btn-width=")
                    .Append(PayPageCosmetic.Default.btn_width)
                    .Append(';');
            }
            if (PayPageCosmetic.Default.btn_border_top_left_radius != "")
            {
                sb.Append("&btn-border-top-left-radius=")
                    .Append(PayPageCosmetic.Default.btn_border_top_left_radius)
                    .Append(';');
            }
            if (PayPageCosmetic.Default.btn_border_top_right_radius != "")
            {
                sb.Append("&btn-border-top-right-radius=")
                    .Append(PayPageCosmetic.Default.btn_border_top_right_radius)
                    .Append(';');
            }
            if (PayPageCosmetic.Default.btn_border_bottom_left_radius != "")
            {
                sb.Append("&btn-border-bottom-left-radius=")
                    .Append(PayPageCosmetic.Default.btn_border_bottom_left_radius)
                    .Append(';');
            }
            if (PayPageCosmetic.Default.btn_border_bottom_right_radius != "")
            {
                sb.Append("&btn-border-bottom-right-radius=")
                    .Append(PayPageCosmetic.Default.btn_border_bottom_right_radius)
                    .Append(';');
            }
            if (PayPageCosmetic.Default.btn_border_color != "")
            {
                sb.Append("&btn-border-color=")
                    .Append(PayPageCosmetic.Default.btn_border_color)
                    .Append(';');
            }
            if (PayPageCosmetic.Default.btn_border_style != "")
            {
                sb.Append("&btn-border-style")
                    .Append(PayPageCosmetic.Default.btn_border_style)
                    .Append(';');
            }
            if (PayPageCosmetic.Default.section_header_font_size != "")
            {
                sb.Append("&section-header-font-size=")
                    .Append(PayPageCosmetic.Default.section_header_font_size)
                    .Append(';');
            }
            if (PayPageCosmetic.Default.input_field_height != "")
            {
                sb.Append("&input-field-height=")
                    .Append(PayPageCosmetic.Default.input_field_height)
                    .Append(';');
            }
            if (PayPageCosmetic.Default.line_spacing_size != "")
            {
                sb.Append("&line-spacing-size=")
                    .Append(PayPageCosmetic.Default.line_spacing_size)
                    .Append(';');
            }

            return sb.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder combiner = new StringBuilder();
            combiner.Append(Globals.Default.CustomParameters)
                .Append(PayPageBrandingCombiner());
            Globals.Default.CustomParameters = combiner.ToString();

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
