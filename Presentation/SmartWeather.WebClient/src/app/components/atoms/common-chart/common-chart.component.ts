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
  readonly MAX_POINTS = 100;

  constructor() {

  }

  ngOnInit(): void {
    this.updateChart();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['seriesData']) {
      setTimeout(() => {
        this.updateChart();
      }, 500);
    }
  }

  private sampleData(data: [Date, number][], maxPoints: number): any[] {
    console.log("Sampling data : ");
    console.log(data.length);

    if (data.length <= maxPoints) return data;

    const step = Math.ceil(data.length / maxPoints);
    console.log("Sampled data : ", data.filter((_, index) => index % step === 0));
    return data.filter((_, index) => index % step === 0);
  }

  private updateChart() {
    console.log("Update !!!", this.seriesData);
    console.log("Update !!!", this.seriesData.map(series => series.name));

    this.options = {
      legend: {
        data: this.seriesData.map(series => series.name),
        align: 'left',
      },
      tooltip: {
        trigger: 'axis', // Affiche les infos pour toutes les séries au survol
        axisPointer: {
          type: 'cross' // Ligne de suivi pour mieux lire les valeurs
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
      series: this.seriesData.map(serie => ({
        name: serie.name,
        type: this.chartType || 'line',
        data: this.sampleData(serie.data.sort((a, b) => a.x.getTime() - b.x.getTime()).map(p => [p.x, p.y]), this.MAX_POINTS),
        smooth: true,
        sampling: 'average',
        itemStyle: {
          color: serie.color
        }
      })),
      dataZoom: [
        {
          type: 'inside', // Zoom avec la molette ou le pinch sur mobile
          start: 0,
          end: 100,
        },
        {
          type: 'slider', // Barre de zoom contrôlable
          show: true,
          start: 0,
          end: 100,
        },
      ],
      animationEasing: 'elasticOut',
      animationDelayUpdate: idx => idx * 5,
    };

    console.log(this.options);

  }

  // private updateChart() {
  //   console.log("Update !!!", this.seriesData)
  //   this.options = {
  //     legend: {
  //       data: this.seriesData.map(series => (series.name)),
  //       align: 'left',
  //     },
  //     tooltip: {},
  //     xAxis: {
  //       type: 'time',
  //       boundaryGap: false,
  //       axisLabel: {
  //         formatter: (value: number) => new Date(value).toLocaleTimeString()
  //       }
  //     },
  //     yAxis: { type: 'value' },
  //     series: this.seriesData.map(series => ({
  //       name: series.name,
  //       type: this.chartType,
  //       data: series.data.map(p => p.y),
  //       smooth: true,
  //     })),
  //     animationEasing: 'elasticOut',
  //     animationDelayUpdate: idx => idx * 5,
  //   };
  // }



}

