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

            ParseQueryStringFromCustomParameters();
        }
        public void ParseQueryStringFromCustomParameters()
        {
            NameValueCollection keyPairs = HttpUtility.ParseQueryString(Globals.Default.CustomParameters);

            PayPageCosmetic.Default.font_family = keyPairs.Get("font-family");
            PayPageCosmetic.Default.font_size = keyPairs.Get("font-size");
            PayPageCosmetic.Default.color = keyPairs.Get("color");
            PayPageCosmetic.Default.background_color = keyPairs.Get("background-color");
            PayPageCosmetic.Default.btn_color = keyPairs.Get("btn_color");
            PayPageCosmetic.Default.btn_background_color = keyPairs.Get("btn-background-color");
            PayPageCosmetic.Default.btn_height = keyPairs.Get("btn-height");
            PayPageCosmetic.Default.btn_width = keyPairs.Get("btn-width");
            PayPageCosmetic.Default.btn_border_top_left_radius = keyPairs.Get("btn-border-top-left-radius");
            PayPageCosmetic.Default.btn_border_top_right_radius = keyPairs.Get("btn-border-top-right-radius");
            PayPageCosmetic.Default.btn_border_bottom_left_radius = keyPairs.Get("btn-border-bottom-left-radius");
            PayPageCosmetic.Default.btn_border_bottom_right_radius = keyPairs.Get("btn-border-bottom-right-radius");
            PayPageCosmetic.Default.btn_border_color = keyPairs.Get("btn-border-color");
            PayPageCosmetic.Default.btn_border_style = keyPairs.Get("btn-border-style");
            PayPageCosmetic.Default.section_header_font_size = keyPairs.Get("section-header-font-size");
            PayPageCosmetic.Default.line_spacing_size = keyPairs.Get("line-spacing-size");
            PayPageCosmetic.Default.input_field_height = keyPairs.Get("input-field-height");

            PayPageCosmetic.Default.Save();
        }
    }
}
