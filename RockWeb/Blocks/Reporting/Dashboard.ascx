<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Dashboard.ascx.cs" Inherits="RockWeb.Blocks.Reporting.Dashboard" %>
    
<asp:UpdatePanel ID="upDashboard" runat="server">
    <ContentTemplate>
        <span id="jqWidth">0</span>
        <span id="jqHeight">0</span>
		<div id="wrap">
			<section class="gridster">
				<ul>
                    
<%--                    <li class="widget" data-row="1" data-col="1" data-sizex="1" data-sizey="1"></li>
                    <li class="widget" data-row="2" data-col="1" data-sizex="1" data-sizey="1"></li>
                    <li class="widget" data-row="3" data-col="1" data-sizex="1" data-sizey="1"></li>
 
                    <li class="widget" data-row="1" data-col="2" data-sizex="2" data-sizey="1"></li>
                    <li class="widget" data-row="2" data-col="2" data-sizex="2" data-sizey="2"></li>
 
                    <li class="widget" data-row="1" data-col="4" data-sizex="1" data-sizey="1"></li>
                    <li class="widget" data-row="2" data-col="4" data-sizex="2" data-sizey="1"></li>
                    <li class="widget" data-row="3" data-col="4" data-sizex="1" data-sizey="1"></li>
 
                    <li class="widget" data-row="1" data-col="5" data-sizex="1" data-sizey="1"></li>
                    <li class="widget" data-row="3" data-col="5" data-sizex="1" data-sizey="1"></li>--%>

                    <li class="widget" data-row="1" data-col="1" data-sizex="1" data-sizey="1">1</li>
                    <li class="widget" data-row="2" data-col="1" data-sizex="1" data-sizey="1">2</li>
                    <li class="widget" data-row="3" data-col="1" data-sizex="1" data-sizey="1">3</li>
                    <li class="widget" data-row="4" data-col="1" data-sizex="1" data-sizey="1">4</li>
                    <li class="widget" data-row="1" data-col="2" data-sizex="1" data-sizey="1">5</li>
                    <li class="widget" data-row="2" data-col="2" data-sizex="1" data-sizey="1">6</li>
                    <li class="widget" data-row="3" data-col="2" data-sizex="1" data-sizey="1">7</li>
                    <li class="widget" data-row="4" data-col="2" data-sizex="1" data-sizey="1">8</li>
                    <li class="widget" data-row="1" data-col="3" data-sizex="1" data-sizey="1">9</li>
                    <li class="widget" data-row="2" data-col="3" data-sizex="1" data-sizey="1">10</li>
                    <li class="widget" data-row="3" data-col="3" data-sizex="1" data-sizey="1">11</li>
                    <li class="widget" data-row="4" data-col="3" data-sizex="1" data-sizey="1">12</li>
                    <li class="widget" data-row="1" data-col="4" data-sizex="1" data-sizey="1">13</li>
                    <li class="widget" data-row="2" data-col="4" data-sizex="1" data-sizey="1">14</li>
                    <li class="widget" data-row="3" data-col="4" data-sizex="1" data-sizey="1">15</li>
                    <li class="widget" data-row="4" data-col="4" data-sizex="1" data-sizey="1">16</li>
                    <li class="widget" data-row="5" data-col="1" data-sizex="4" data-sizey="4">17</li>
				</ul>
			</section> <!-- end "gridster" -->
		</div> <!-- end "wrap" -->
        <div class="action">
            <asp:LinkButton ID="lbAdd" runat="server" OnClick="lbAdd_Click" CssClass="add btn" ToolTip="Add a metric to this block."><i class="icon-plus-sign"></i></asp:LinkButton>
        </div> <!-- end "action" -->
   		<script>
   		    $(function () { //DOM Ready
   		        $(".gridster ul").gridster({
   		            widget_margins: [10, 10],
   		            widget_base_dimensions: [320, 166],
   		            //widget_base_dimensions: [370, 170],
   		            max_size_x: 5,
   		            max_size_y: 5,
   		            extra_cols: 0,
                    autogenerate_stylesheet: true
   		        });
   		    });
   		    function jqUpdateSize() {
   		        // Get the dimensions of the viewport
   		        var width = $(window).width();
   		        var height = $(window).height();

   		        $('#jqWidth').html(width);      // Display the width
   		        $('#jqHeight').html(height);    // Display the height
                /*
   		        if (width >= 1200) {
   		            $("#wrap").removeClass().addClass("div-1200");
   		        } else if (width < 1200 && width >= 1024) {
   		            $("#wrap").removeClass().addClass("div-1024");
   		        } else if (width < 1024 && width >= 768) {
   		            $("#wrap").removeClass().addClass("div-768");
   		        } else if (width < 768 && width >= 640) {
   		            $("#wrap").removeClass().addClass("div-640");
   		        } else if (width < 640 && width >= 480) {
   		            $("#wrap").removeClass().addClass("div-480");
   		        } else if (width < 480) {
   		            $("#wrap").removeClass().addClass("div-320");
   		        }
                */
   		    };
   		    $(document).ready(jqUpdateSize);    // When the page first loads
   		    $(window).resize(jqUpdateSize);     // When the browser changes size
   		    //function pageLoad(sender, args) {
   		    //    // find out the size of the browser window and change the width of the wrap class accordingly.
   		    //    $(".wrap").removeClass().addClass("div-320");
   		    //    $(".wrap").removeClass().addClass("div-480");
   		    //    $(".wrap").removeClass().addClass("div-640");
   		    //    $(".wrap").removeClass().addClass("div-768");
   		    //    $(".wrap").removeClass().addClass("div-1024");
   		    //};
		</script>

    </ContentTemplate>
</asp:UpdatePanel>