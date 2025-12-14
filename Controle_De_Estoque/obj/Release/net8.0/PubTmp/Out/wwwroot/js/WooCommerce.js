$(document).ready(function () {
    $(document).on('click', '.btn-editar', function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        $.ajax({
            type: "GET",
            url: `/WooCommerce/Update?id=${id}`,
            success: function (html) {
                $('#conteudoModalEditar').html(html);
                $('#modalEdicao').modal('show');
            }
        });
    });



    $(document).on('submit', '#form-Update', function (e) {
        e.preventDefault();
        $.ajax({
            type: "POST",
            url: "/WooCommerce/UpdateProductItem",
            data: $(this).serialize(),
            success: function () {
                $("#modalEdicao").modal("hide");
                location.reload();
            }
        });
    });
 
});
