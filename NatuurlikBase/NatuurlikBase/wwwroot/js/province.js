var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"Province/GetAll"
        },
        "columns": [
            { data: "provinceName", "width": "15%" },
            { data: "country.countryName", "width": "15%" },
            {
                "data": "provinceId",
                "render": function(data)
                {
                    return `
                     <div class="w-75 btn-group" role="group">
                        <a href="/Province/Upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Edit</a>
                        <a onClick="Delete('/Province/Delete/${data}')" class="btn btn-danger mx-2"><i class="bi bi-trash3-fill"></i> Delete</a>

                    </div>
                    `
                },
                "width": "15%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, proceed with delete '
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
    }
