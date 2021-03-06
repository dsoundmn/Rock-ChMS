﻿//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// Control for selecting a date range
    /// </summary>
    [ToolboxData( "<{0}:DateRangePicker runat=server></{0}:DateRangePicker>" )]
    public class DateRangePicker : CompositeControl, IRockControl
    {
        #region IRockControl implementation

        /// <summary>
        /// Gets or sets the label text.
        /// </summary>
        /// <value>
        /// The label text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The text for the label." )
        ]
        public string Label
        {
            get { return ViewState["Label"] as string ?? string.Empty; }
            set { ViewState["Label"] = value; }
        }

        /// <summary>
        /// Gets or sets the help text.
        /// </summary>
        /// <value>
        /// The help text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The help block." )
        ]
        public string Help
        {
            get
            {
                return HelpBlock != null ? HelpBlock.Text : string.Empty;
            }

            set
            {
                if ( HelpBlock != null )
                {
                    HelpBlock.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RockTextBox"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        [
        Bindable( true ),
        Category( "Behavior" ),
        DefaultValue( "false" ),
        Description( "Is the value required?" )
        ]
        public bool Required
        {
            get { return ViewState["Required"] as bool? ?? false; }
            set { ViewState["Required"] = value; }
        }

        /// <summary>
        /// Gets or sets the required error message.  If blank, the LabelName name will be used
        /// </summary>
        /// <value>
        /// The required error message.
        /// </value>
        public string RequiredErrorMessage
        {
            get
            {
                return RequiredFieldValidator != null ? RequiredFieldValidator.ErrorMessage : string.Empty;
            }

            set
            {
                if ( RequiredFieldValidator != null )
                {
                    RequiredFieldValidator.ErrorMessage = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsValid
        {
            get
            {
                return !Required || RequiredFieldValidator == null || RequiredFieldValidator.IsValid;
            }
        }

        /// <summary>
        /// Gets or sets the help block.
        /// </summary>
        /// <value>
        /// The help block.
        /// </value>
        public HelpBlock HelpBlock { get; set; }

        /// <summary>
        /// Gets or sets the required field validator.
        /// </summary>
        /// <value>
        /// The required field validator.
        /// </value>
        public RequiredFieldValidator RequiredFieldValidator { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangePicker"/> class.
        /// </summary>
        public DateRangePicker()
            : base()
        {
            RequiredFieldValidator = new HiddenFieldValidator();
            HelpBlock = new HelpBlock();
        }

        #region Controls

        /// <summary>
        /// The lower value 
        /// </summary>
        private DatePicker _tbLowerValue;

        /// <summary>
        /// The upper value 
        /// </summary>
        private DatePicker _tbUpperValue;

        #endregion

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // a little javascript to make the daterange picker behave similar to the bootstrap-datepicker demo site's date range picker
            var scriptFormat = @"
$('#{0}').datepicker().on('changeDate', function (ev) {{

    // if the startdate is later than the enddate, change the end date to be startdate+1
    if (ev.date.valueOf() > $('#{1}').data('datepicker').date.valueOf()) {{
        var newDate = new Date(ev.date)
        newDate.setDate(newDate.getDate() + 1);
        $('#{1}').datepicker('update', newDate);

        // disable date selection in the EndDatePicker that are earlier than the startDate
        $('#{1}').datepicker('setStartDate', ev.date);
    }}
    
    // close the start date picker and set focus to the end date
    $('#{0}').data('datepicker').hide();
    $('#{1}')[0].focus();
}});

$('#{1}').datepicker().on('changeDate', function (ev) {{
    // close the enddate picker immediately after selecting an end date
    $('#{1}').data('datepicker').hide();
}});

";

            EnsureChildControls();
            var script = string.Format( scriptFormat, _tbLowerValue.ClientID, _tbUpperValue.ClientID );
            ScriptManager.RegisterStartupScript( this, this.GetType(), "daterange_picker-" + this.ClientID, script, true );
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            Controls.Clear();
            RockControlHelper.CreateChildControls( this, Controls );

            _tbLowerValue = new DatePicker();
            _tbLowerValue.ID = this.ID + "_lower";
            _tbLowerValue.CssClass = "input-width-md";
            Controls.Add( _tbLowerValue );

            _tbUpperValue = new DatePicker();
            _tbUpperValue.ID = this.ID + "_upper";
            _tbUpperValue.CssClass = "input-width-md";
            Controls.Add( _tbUpperValue );
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( this.Visible )
            {
                RockControlHelper.RenderControl( this, writer );
            }
        }

        /// <summary>
        /// This is where you implment the simple aspects of rendering your control.  The rest
        /// will be handled by calling RenderControlHelper's RenderControl() method.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void RenderBaseControl( HtmlTextWriter writer )
        {
            writer.AddAttribute( "class", "form-control-group" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            _tbLowerValue.RenderControl( writer );
            writer.Write( "<span class='to'> to </span>" );
            _tbUpperValue.RenderControl( writer );

            writer.RenderEndTag();
        }

        /// <summary>
        /// Gets or sets the lower value.
        /// </summary>
        /// <value>
        /// The lower value.
        /// </value>
        public DateTime? LowerValue
        {
            get
            {
                EnsureChildControls();
                return _tbLowerValue.SelectedDate;
            }

            set
            {
                EnsureChildControls();
                _tbLowerValue.SelectedDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the upper value.
        /// </summary>
        /// <value>
        /// The upper value.
        /// </value>
        public DateTime? UpperValue
        {
            get
            {
                EnsureChildControls();
                return _tbUpperValue.SelectedDate;
            }

            set
            {
                EnsureChildControls();
                _tbUpperValue.SelectedDate = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [read only]; otherwise, <c>false</c>.
        /// </value>
        public bool ReadOnly
        {
            get
            {
                EnsureChildControls();
                return _tbLowerValue.ReadOnly;
            }

            set
            {
                EnsureChildControls();
                _tbLowerValue.ReadOnly = value;
                _tbUpperValue.ReadOnly = value;
            }
        }

        /// <summary>
        /// Gets or sets the validation group.
        /// </summary>
        /// <value>
        /// The validation group.
        /// </value>
        public string ValidationGroup
        {
            get
            {
                EnsureChildControls();
                return _tbLowerValue.ValidationGroup;
            }

            set
            {
                EnsureChildControls();
                _tbLowerValue.ValidationGroup = value;
                _tbUpperValue.ValidationGroup = value;
            }
        }

        /// <summary>
        /// Gets or sets the lower and upper values by specifying a comma-delimted lower and upper date
        /// </summary>
        /// <value>
        /// The delimited values.
        /// </value>
        public string DelimitedValues
        {
            get
            {
                return string.Format( "{0:d},{1:d}", this.LowerValue, this.UpperValue );
            }
            set
            {
                if ( value != null )
                {
                    string[] valuePair = value.Split( new char[] { ',' }, StringSplitOptions.None );
                    if ( valuePair.Length == 2 )
                    {
                        DateTime result;

                        if ( DateTime.TryParse( valuePair[0], out result ) )
                        {
                            this.LowerValue = result;
                        }
                        else
                        {
                            this.LowerValue = null;
                        }

                        if ( DateTime.TryParse( valuePair[1], out result ) )
                        {
                            this.UpperValue = result;
                        }
                        else
                        {
                            this.UpperValue = null;
                        }
                    }
                    else
                    {
                        this.LowerValue = null;
                        this.UpperValue = null;
                    }
                }
                else
                {
                    this.LowerValue = null;
                    this.UpperValue = null;
                }
            }
        }

        /// <summary>
        /// Formats the delimited values.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string FormatDelimitedValues( string value )
        {
            try
            {
                if ( value != null )
                {
                    if ( value.StartsWith( "," ) )
                    {
                        string upperValue = DateTime.Parse( value.Substring( 1 ) ).Date.ToShortDateString();
                        return string.Format( "through {0}", upperValue );
                    }
                    else if ( value.EndsWith( "," ) )
                    {
                        string lowerValue = DateTime.Parse( value.Substring( 0, value.Length - 1 ) ).Date.ToShortDateString();
                        return string.Format( "from {0}", lowerValue );
                    }
                    else
                    {
                        string[] valuePair = value.Split( new char[] { ',' }, StringSplitOptions.None );
                        if ( valuePair.Length == 2 )
                        {
                            string lowerValue = string.IsNullOrWhiteSpace( valuePair[0] ) ? Rock.Constants.None.TextHtml : DateTime.Parse( valuePair[0] ).Date.ToShortDateString();
                            string upperValue = string.IsNullOrWhiteSpace( valuePair[1] ) ? Rock.Constants.None.TextHtml : DateTime.Parse( valuePair[1] ).Date.ToShortDateString();
                            return string.Format( "{0} to {1}", lowerValue, upperValue );
                        }
                    }
                }

                return null;
            }
            catch 
            {
                return null;  
            }
        }
    }
}
