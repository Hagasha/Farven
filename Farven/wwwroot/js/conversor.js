$(document).ready(function () {
    // Carregar as moedas disponíveis
    $.ajax({
        url: "https://api.frankfurter.app/currencies",
        success: function (data) {
            $.each(data, function (code, name) {
                $("#from, #to").append($("<option>", {
                    value: code,
                    text: `${code} - ${name}`
                }));
            });
        },
        error: function () {
            $("#result").html("Erro ao carregar as moedas.");
        }
    });
});

function convertCurrency() {
    var fromCurrency = $("#from").val();
    var toCurrency = $("#to").val();
    var amount = $("#amount").val();

    if (fromCurrency && toCurrency && amount) {
        $.ajax({
            url: `https://api.frankfurter.app/latest?base=${fromCurrency}&symbols=${toCurrency}`,
            success: function (data) {
                const rate = data.rates[toCurrency];
                const convertedAmount = (amount * rate).toFixed(2);
                $("#result").html(`${amount} ${fromCurrency} = ${convertedAmount} ${toCurrency}`);
            },
            error: function () {
                $("#result").html("Ocorreu um erro na conversão.");
            }
        });
    } else {
        $("#result").html("Por favor, preencha todos os campos.");
    }
}