$(document).ready
(
    function () {

        var table = $('#jq_table').DataTable();

        //test
        //div_task_end_date

        var allData = table.rows().data();
        var Nodes = $("#jq_table").dataTable().fnGetNodes();
       // var StatusValue = Nodes[0].children[1].textContent;
        
        // get the task array
        var col = $('.task_row');

        //pointer to the icon div
        var StatusIconDiv = col.find('.IconDiv');

        for (var i = 0; i < Nodes.length; i++)
        {
            ////set ok picture
            //var green_icon = document.createElement('img');
            //green_icon.src = '/images/Ok-48.png';

            ////set bad picture
            //var red_icon = document.createElement('img');
            //red_icon.src = '/images/Cancel-48.png';


            if (allData[i][1] == "done")
            {
                var td = Nodes[i].children[6].children[0].src = "/images/Ok-48.png";
            }
            else
            {
                var td = Nodes[i].children[6].children[0].src = "/images/Cancel-48.png";
            }
        }


        $('.date-picker').datepicker();

        $('.date-picker').click(function ()
        {
            var x = 0;
        })
        

    });








$('#jq_table').on('page.dt', function () {
    // get the task array
    var col = $('.task_row');
});


$('#jq_table').on('order.dt', function () {
    // get the task array
    var col = $('.task_row')
});


