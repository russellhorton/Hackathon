
var Epilexy = {
    Reports: {
        Chart: {
            Global: {
                LatestHeartRateTimeStamp: 0,
                LatestMotionSensorTimeStamp: 0,
                NoOfEmptyResponses: 0,
                EmptyResponseThreshold: 20,
                MaxDataPoints: 20,
                FirstUpdate: true,
                UpdateInterval: 1000,
                MotionSensorChart: "",
                HeartRateChart: "",
                HeartRateData: {},
                HeartRateDataSet :[],
                HeartRateDataLabels : [],
                MotionData: {},
                MotionDataXDataSet: [],
                MotionDataLabels: [],
                MotionDataYDataSet: [],
                MotionDataZDataSet: []
            },
            Init: function() {
                Chart.defaults.global.responsive = true;
                Chart.defaults.global.animation = true;
                Chart.defaults.global.animationSteps = 20;

                Epilexy.Reports.Chart.Data.LoadInitialData();
                Epilexy.Reports.Chart.Data.GetLatest();
            },
            InitHeartRateChart: function () {

                var Global = Epilexy.Reports.Chart.Global,
                    chartContext = $("#heartRateChart")[0].getContext("2d");
                Global.HeartRateData = {
                    labels: Global.HeartRateDataLabels,
                    datasets: [
                        {
                            label: "Heart rate",
                            fillColor: "rgba(220,220,220,0.2)",
                            strokeColor: "rgba(220,220,220,1)",
                            pointColor: "rgba(220,220,220,1)",
                            pointStrokeColor: "#fff",
                            pointHighlightFill: "#fff",
                            pointHighlightStroke: "rgba(220,220,220,1)",
                            data: Global.HeartRateDataSet
                        }
                    ]
                };
                Global.HeartRateChart = new Chart(chartContext).Line(Global.HeartRateData);
            },
            InitMotionSensorChart: function (){

                var Global = Epilexy.Reports.Chart.Global,
                    chartContext = $("#motionChart")[0].getContext("2d");

                    Global.MotionData = {
                        labels: Global.MotionDataLabels,
                        datasets: [
                        {
                            label: "X Value",
                            fillColor: "rgba(120,120,120,0.2)",
                            strokeColor: "rgba(120,120,120,1)",
                            pointColor: "rgba(120,120,120,1)",
                            pointStrokeColor: "#fff",
                            pointHighlightFill: "#fff",
                            pointHighlightStroke: "rgba(120,120,120,1)",
                            data: Global.MotionDataXDataSet
                        },
                        {
                            label: "Y Value",
                            fillColor: "rgba(160,160,160,0.2)",
                            strokeColor: "rgba(160,160,160,1)",
                            pointColor: "rgba(160,160,160,1)",
                            pointStrokeColor: "#fff",
                            pointHighlightFill: "#fff",
                            pointHighlightStroke: "rgba(160,160,160,1)",
                            data: Global.MotionDataYDataSet
                        },
                        {
                            label: "Z Value",
                            fillColor: "rgba(200,120,120,0.2)",
                            strokeColor: "rgba(200,120,120,1)",
                            pointColor: "rgba(200,120,120,1)",
                            pointStrokeColor: "#fff",
                            pointHighlightFill: "#fff",
                            pointHighlightStroke: "rgba(200,120,120,1)",
                            data: Global.MotionDataZDataSet
                        }
                        ]
                    };
                    Global.MotionSensorChart = new Chart(chartContext).Line(Global.MotionData);
            },
            Data: {
                GetLatest: function () {

                    var Global = Epilexy.Reports.Chart.Global,
                        updateGraph = setInterval(function () {

                            if (Global.NoOfEmptyResponses > Global.EmptyResponseThreshold) {
                                clearInterval(updateGraph);
                            }

                            var timeSince = "";
        
                            if (Global.LatestHeartRateTimeStamp != 0 && Global.LatestMotionSensorTimeStamp != 0) {
                                if (Global.LatestHeartRateTimeStamp > Global.LatestMotionSensorTimeStamp) {
                                    timeSince = Global.LatestHeartRateTimeStamp;
                                } else {
                                    timeSince = Global.LatestMotionSensorTimeStamp;
                                }
                            } else {
                                if (Global.FirstUpdate) {
                                    //timeSince = Epilexy.Reports.Util.GetDateTime();

                                } else {
                                    Global.FirstUpdate = false;
                                }
                            }

                            $.ajax({
                                type: "POST",
                                url: "http://epilepsysite.local/umbraco/api/ReportApi/ReturnLatest/",
                                data: { UserId: Epilexy.Reports.Util.QueryString()["userid"], TimeSince: timeSince },
                                success: Epilexy.Reports.Chart.Data.HandleResponse
                            });
        
                        }, Global.UpdateInterval);

                },
                LoadInitialData: function (){
                    $.ajax({
                        type: "POST",
                        url: "http://epilepsysite.local/umbraco/api/ReportApi/ReturnLast20/",
                        data: { UserId: Epilexy.Reports.Util.QueryString()["userid"] },
                        success: Epilexy.Reports.Chart.Data.HandleInitialDataResponse
                    });
                },
                HandleInitialDataResponse: function (data){
                    data = JSON.parse(data);

                    //console.log(data);

                    var Global = Epilexy.Reports.Chart.Global;

                    for (var n = 0; n <= data.heartRateItems.length -1; n++) {
                        Global.HeartRateDataSet.push(data.heartRateItems[n].HeartRate);
                        Global.HeartRateDataLabels.push(data.heartRateItems[n].DateTime.split("T")[1]);
                    }

                    for (var n = 0; n <= data.motionSensorItems.length - 1; n++) {
                        Global.MotionDataXDataSet.push(data.motionSensorItems[n].XValue);
                        Global.MotionDataYDataSet.push(data.motionSensorItems[n].YValue);
                        Global.MotionDataZDataSet.push(data.motionSensorItems[n].ZValue);
                        Global.MotionDataLabels.push(data.motionSensorItems[n].DateTime.split("T")[1]);
                    }

                    Global.LatestHeartRateTimeStamp = data.heartRateItems[0].DateTime;
                    Global.LatestMotionSensorTimeStamp = data.motionSensorItems[0].DateTime;

                    Epilexy.Reports.Chart.InitHeartRateChart();
                    Epilexy.Reports.Chart.InitMotionSensorChart();
                },
                HandleResponse: function (data) {

                    //console.log("data: " + data);

                    data = JSON.parse(data);

                    var Global = Epilexy.Reports.Chart.Global;

                    if (data.heartRateItems.length > 0) {
                        Global.LatestHeartRateTimeStamp = data.heartRateItems[0].DateTime;
                    }

                    if (data.motionSensorItems.length > 0) {
                        Global.LatestMotionSensorTimeStamp = data.motionSensorItems[0].DateTime;
                    }

                    if (data.heartRateItems.length == 0 && data.motionSensorItems == 0) {
                        Global.NoOfEmptyResponses++;
                    }

                    // heart rate
                    for (var n = 0; n < data.heartRateItems.length; n++) {
                        var item = data.heartRateItems[n];
                        var addItem = true;

                        item.DateTime = item.DateTime.split("T")[1];

                        if (!Epilexy.Reports.Chart.Data.DoesLabelExist(Global.HeartRateChart, item.DateTime)) {

                            Global.HeartRateChart.addData([item.HeartRate], [item.DateTime]);

                            if (Global.HeartRateData.labels.length > Global.MaxDataPoints) {
                                Global.HeartRateChart.removeData();
                            }

                        }
                    }

                    // motion
                    for (var n = 0; n < data.motionSensorItems.length; n++) {
                        var item = data.motionSensorItems[n];
                        var addItem = true;

                        item.DateTime = item.DateTime.split("T")[1];

                        if (!Epilexy.Reports.Chart.Data.DoesLabelExist(Global.MotionSensorChart, item.DateTime)) {

                            Global.MotionSensorChart.addData([item.XValue, item.YValue, item.ZValue], [item.DateTime]);

                            if (Global.MotionData.labels.length > Global.MaxDataPoints) {
                                Global.MotionSensorChart.removeData();
                            }
                        }
                    }
                },
                DoesLabelExist: function (chart, label) {

                    if (typeof(chart.label) == "undefined") {
                        return false;
                    }

                    for (var i = 0; i < chart.labels.length; i++) {
                        if (chart.labels[i][0] == label) {
                            return true;
                        }
                    }

                    return false;
                }
            }
        },
        Table: {
            Init: function () {
                if ($('#motiontable')) {
                    $('#motiontable').DataTable();
                }

                if ($('#heartratetable')) {
                    $('#heartratetable').DataTable();
                }
            }
        },
        Util: {
            QueryString: function()
            {
                var vars = [], hash;
                var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
                for(var i = 0; i < hashes.length; i++)
                {
                    hash = hashes[i].split('=');
                    vars.push(hash[0]);
                    vars[hash[0]] = hash[1];
                }
                return vars;
            },
            GetDateTime: function() {
                var now = new Date();
                var year = now.getFullYear();
                var month = now.getMonth() + 1;
                var day = now.getDate();
                var hour = now.getHours();
                var minute = now.getMinutes();
                var second = now.getSeconds();
                if (month.toString().length == 1) {
                    var month = '0' + month;
                }
                if (day.toString().length == 1) {
                    var day = '0' + day;
                }
                if (hour.toString().length == 1) {
                    var hour = '0' + hour;
                }
                if (minute.toString().length == 1) {
                    var minute = '0' + minute;
                }
                if (second.toString().length == 1) {
                    var second = '0' + second -3;
                }
                var dateTime = year + '-' + month + '-' + day + 'T' + hour + ':' + minute + ':' + second;
                return dateTime;
            }   
        }
    }
}

$(document).ready(function () {
    Epilexy.Reports.Chart.Init();
    Epilexy.Reports.Table.Init();
});