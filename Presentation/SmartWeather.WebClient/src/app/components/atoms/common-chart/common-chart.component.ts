import { CommonModule } from '@angular/common';
import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { EChartsCoreOption } from 'echarts/core';
import { NgxEchartsDirective, provideEchartsCore } from 'ngx-echarts';
import * as echarts from 'echarts/core';
import { BarChart } from 'echarts/charts';
import { GridComponent } from 'echarts/components';
import { CanvasRenderer } from 'echarts/renderers';
import { LineChart } from 'echarts/charts';
import { TooltipComponent } from 'echarts/components';
import { LegendComponent } from 'echarts/components';
import { DataZoomComponent } from 'echarts/components';
import { TimeSerie } from '@models/ui/charting';
echarts.use([DataZoomComponent, BarChart, GridComponent, CanvasRenderer, LineChart, TooltipComponent, LegendComponent]);

@Component({
  selector: 'app-common-chart',
  imports: [CommonModule, NgxEchartsDirective],
  templateUrl: './common-chart.component.html',
  styleUrl: './common-chart.component.css',
  providers: [
    provideEchartsCore({ echarts }),
  ]
})
export class CommonChartComponent implements OnInit, OnChanges {

  @Input() seriesData: TimeSerie[] = [];
  @Input() chartId: string = "";
  @Input() chartType: string = "";

  options: EChartsCoreOption | null = null;
  chartInstance: any;
  displayedData: TimeSerie[] = [];

  readonly MAX_POINTS = 200;

  constructor() {

  }

  ngOnInit(): void {
    this.updateChart();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['seriesData']) {
      setTimeout(() => {
        this.displayedData = structuredClone(this.seriesData);
        this.updateChart();
      }, 500);
    }
  }

  onChartInit(chartInstance: any) {
    this.chartInstance = chartInstance;

    // for the moment let the zoom sampling appart
    // chartInstance.on('dataZoom', (params: any) => {
    //   const start = params.start;
    //   const end = params.end;

    //   console.log(`Zoom: ${start}% → ${end}%`);

    //   console.log("********************")
    //   console.log("vvvvvvvvvvvvvvvvvvvvv")
    //   console.log(this.displayedData)

    //   this.seriesData.forEach((serie, index) => {
    //     const startIndex = Math.floor((start / 100) * serie.data.length);
    //     const endIndex = Math.ceil((end / 100) * serie.data.length);

    //     const filteredData: [Date, number][] = serie.data.slice(startIndex, endIndex);

    //     // const filteredData: [Date, number][] = serie.data.filter((point) => {
    //     //   const timestamp = new Date(point.x).getTime();
    //     //   return timestamp >= start && timestamp <= end;
    //     // }).map(p => [p.x, p.y]);
    //     this.displayedData[index].data = filteredData;
    //   });
    //   console.log(this.displayedData)

    //   console.log("^^^^^^^^^^^^^^^^^^^^")
    //   console.log("********************")
    //   this.updateChart();
    //   chartInstance.setOption(this.options, true);
    // });
  }

  private sampleData(data: [Date, number][], maxPoints: number): any[] {
    if (data.length <= maxPoints) return data;

    const step = Math.ceil(data.length / maxPoints);
    const sampledData: [Date, number][] = [];

    for (let i = 0; i < data.length; i += step) {
      const chunk = data.slice(i, i + step);
      const avgX = new Date(
        chunk.reduce((sum, point) => sum + point[0].getTime(), 0) / chunk.length
      );
      const avgY =
        chunk.reduce((sum, point) => sum + point[1], 0) / chunk.length;

      sampledData.push([avgX, avgY]);
    }

    return sampledData;
  }


  private updateChart() {
    this.options = {
      legend: {
        data: this.displayedData.map(series => series.name),
        align: 'left',
      },
      tooltip: {
        trigger: 'axis',
        axisPointer: {
          type: 'cross'
        }
      },
      xAxis: {
        type: 'time',
        boundaryGap: false,
        axisLabel: {
          formatter: (value: number) => new Date(value).toLocaleTimeString()
        }
      },
      yAxis: { type: 'value' },
      series: this.displayedData.map(serie => ({
        name: serie.name,
        type: this.chartType || 'line',
        data: this.sampleData(serie.data.sort((a, b) => a[0].getTime() - b[0].getTime()), this.MAX_POINTS),
        smooth: true,
        sampling: 'average',
        treshold: 10,
        itemStyle: {
          color: serie.color
        }
      })),
      dataZoom: [
        {
          type: 'inside',
          start: 0,
          end: 100,
        },
        {
          type: 'slider',
          show: true,
          start: 0,
          end: 100,
        },
      ],
      animationEasing: 'elasticOut',
      animationDelayUpdate: idx => idx * 5,
    };
    this.chartInstance
  }
}