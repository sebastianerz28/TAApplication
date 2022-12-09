/**
 * Author:    Sebastian Ramirez
 * Partner:   Noah Carlson
 * Date:      09/27/2022
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and [Noah Carlson/Sebastian Ramirez] - This work may not be copied for use in Academic Coursework.
 *
 * I, Sebastian Ramirez, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents
 *      This File contains the java script for the role changing functionality of the User roles page.
 */

function Change_Role(user_id, role) {
    $.post(
        {
            url: "/Admin/Change_Role",
            data: {
                user_id: user_id,
                role: role,
            }
        })
        .done(function(data) {
            console.log("Sample of data:", data);
        });
}

function GetEnrollmentData(start, end, dept, number) {
    
    spinner.hidden = false;
    $.post(
        {
            url: "/Admin/GetEnrollmentData",
            data: {
                start: start,
                end: end,
                dept: dept,
                number: number
            }
        })
        .done(function(data) {
            const d = JSON.parse(data);
            let points = [];
            for (let i = 0; i < d.list.length; i++) {
                points.push(d.list[i].TotalEnrollment);
            }
            console.log(points);

            let date = d.list[0].LastUpdated;
            startDates.push(date);
            chart.addSeries({
                name: d.list[0].Course,
                data: points

            });
            for (let i = 0; i < startDates.length; i++) {
                chart.series[i].update({
                    pointStart: Date.UTC(parseInt(startDates[i].substring(0, 4)), parseInt(startDates[i].substring(5, 7)) - 1, parseInt(startDates[i].substring(8, 10)))
                });
            }
            spinner.hidden = true;
        });
    
}

var startDates = [];

var earliest = null;

function pullValues() {
    spinner.hidden = false;


    var start = document.getElementById("start-date").value;
    var end = document.getElementById("end-date").value;
    var course = document.getElementById("course-name").value;
    var deptNum = course.split(" ")
    var data = GetEnrollmentData(start, end, deptNum[0], deptNum[1]);
    console.log(data);
}
let spinner = document.getElementById("spinner");
spinner.hidden = false;

Highcharts.theme = {
    colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572',
        '#FF9655', '#FFF263', '#6AF9C4'],
    chart: {
        backgroundColor: '#6B6B6B',
    },
    title: {
        style: {
            color: '#ECECEC',
            font: 'bold 16px "Trebuchet MS", Verdana, sans-serif'
        }
    },
    subtitle: {
        style: {
            color: '#666666',
            font: 'bold 12px "Trebuchet MS", Verdana, sans-serif'
        }
    },
    legend: {
        itemStyle: {
            font: '9pt Trebuchet MS, Verdana, sans-serif',
            color: '#D2D2D2'
        },
        itemHoverStyle: {
            color: 'white'
        }
    },
    xAxis: {
        labels: {
            style: {
                color: '#ECECEC'
            }
        },
        title: {
            style: {
                color: '#ECECEC'
            }
        }
    },
    yAxis: {
        labels: {
            style: {
                color: '#ECECEC'
            }
        },
        title: {
            style: {
                color: '#ECECEC'
            }
        }
    }
};
// Apply the theme
Highcharts.setOptions(Highcharts.theme);

var chart = new Highcharts.chart('container',
    {
        title: {
            text: 'Course Enrollment Trends',
            align: 'left'
        },


        yAxis: {
            title: {
                text: 'Number of Students'
            }
        },

        xAxis: {
            title: {
                text: 'Time'
            },
            accessibility: {
                rangeDescription: 'Range: Nov 1 - Nov 30'
            },
            type: 'datetime',
            dateTimeLabelFormats: {
                day: '%d %b %Y' //ex- 01 Jan 2016
            }
        },

        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
        },

        plotOptions: {
            series: {
                label: {
                    connectorAllowed: false
                },
                pointStart: Date.UTC(2022, 10, 1),
                pointInterval: 24 * 3600 * 1000
            }
        },

        series: [],

        responsive: {
            rules: [
                {
                    condition: {
                        maxWidth: 500
                    },
                    chartOptions: {
                        legend: {
                            layout: 'horizontal',
                            align: 'center',
                            verticalAlign: 'bottom'
                        }
                    }
                }
            ]
        }

    });


spinner.hidden = true;
