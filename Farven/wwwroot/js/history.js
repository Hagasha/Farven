$(document).ready(function () {
    // Carregar moedas disponíveis
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
            $("#historyResult").html("Erro ao carregar as moedas.");
        }
    });
});

function fetchHistory() {
    var fromCurrency = $("#from").val();
    var toCurrency = $("#to").val();
    var startDate = $("#startDate").val();
    var endDate = $("#endDate").val();

    if (fromCurrency && toCurrency && startDate && endDate) {
        $.ajax({
            url: `https://api.frankfurter.app/${startDate}..${endDate}?base=${fromCurrency}&symbols=${toCurrency}`,
            success: function (data) {
                displayHistory(data.rates, fromCurrency, toCurrency);
            },
            error: function () {
                $("#historyResult").html("Erro ao obter o histórico.");
            }
        });
    } else {
        $("#historyResult").html("Por favor, preencha todos os campos.");
    }
}

function displayHistory(rates, fromCurrency, toCurrency) {
    var resultHtml = `<h4>Histórico de ${fromCurrency} para ${toCurrency}</h4><ul>`;
    for (var date in rates) {
        resultHtml += `<li>${date}: ${rates[date][toCurrency]}</li>`;
    }
    resultHtml += `</ul>`;
    $("#historyResult").html(resultHtml);
}