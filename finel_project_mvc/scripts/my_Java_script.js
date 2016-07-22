$(document).ready
(
    function () {
        $('#jq_table').DataTable();


        // get the task array
        var col = $('.task_row');

        //pointer to the icon div
        var StatusIconDiv = col.find('.IconDiv');

        for (var i = 0; i < col.length; i++)
        {
            //get the status value fro the current task
            var TaskStatus = col[i].getAttribute("id");

            //set ok picture
            var green_icon = document.createElement('img');
            green_icon.src = '/images/Ok-48.png';

            //set bad picture
            var red_icon = document.createElement('img');
            red_icon.src = '/images/Cancel-48.png';


            if (TaskStatus == "done      ")
            {
                StatusIconDiv[i].appendChild(green_icon);
            }
            else
            {
                StatusIconDiv[i].appendChild(red_icon);
            }
        }

    });

