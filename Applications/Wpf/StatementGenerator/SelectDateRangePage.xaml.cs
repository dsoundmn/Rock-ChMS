﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace Rock.Apps.StatementGenerator
{
    /// <summary>
    /// Interaction logic for SelectDateRangePage.xaml
    /// </summary>
    public partial class SelectDateRangePage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectDateRangePage"/> class.
        /// </summary>
        public SelectDateRangePage()
        {
            InitializeComponent();
            if ( !dpStartDate.SelectedDate.HasValue )
            {
                DateTime firstDayOfYear = new DateTime(DateTime.Now.Year, 1, 1);
                dpStartDate.SelectedDate = firstDayOfYear;
            }

            if ( !dpEndDate.SelectedDate.HasValue )
            {
                dpEndDate.SelectedDate = DateTime.Now.Date;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnNext_Click( object sender, RoutedEventArgs e )
        {
            if ( dpEndDate.SelectedDate < dpStartDate.SelectedDate )
            {
                lblWarning.Content = "Start date must be earlier than end date";
                lblWarning.Visibility = Visibility.Visible;
                return;
            }

            if ( !dpStartDate.SelectedDate.HasValue )
            {
                lblWarning.Content = "Please select a start date";
                lblWarning.Visibility = Visibility.Visible;
                return;
            }

            ReportOptions.Current.StartDate = dpStartDate.SelectedDate.Value;

            if ( dpEndDate.SelectedDate.HasValue )
            {
                // set EndDate to 1 day ahead since user would expect the entire full day of enddate to be included
                ReportOptions.Current.EndDate = dpEndDate.SelectedDate.Value.AddDays( 1 );
            }
            else
            {
                ReportOptions.Current.EndDate = null;
            }

            SelectLayoutPage nextPage = new SelectLayoutPage();
            this.NavigationService.Navigate( nextPage );
        }

        /// <summary>
        /// Handles the Click event of the btnBack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnBack_Click( object sender, RoutedEventArgs e )
        {
            this.NavigationService.GoBack();
        }
    }
}
