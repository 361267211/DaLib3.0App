function cqu_lib_data_temp1() {
  var template_html = `<div class="index-warp-box"><div class="index-data-box">
  <div class="index-data-list">
    <h5>本周入馆人数</h5>
    <div class="index-data-list-cur">
      <img :src="imgPath+'cqu/icon-porson.png'" alt="">
      <span>{{cqu_list.weeklyVisitUserCount}}</span>
    </div>
    <div class="index-data-list-pre">上周入馆人数 <span>{{cqu_list.previousWeeklyVisitUserCount}}</span></div>
  </div>
  <div class="index-data-list index-data-list2">
    <h5>本周访问量</h5>
    <div class="index-data-list-cur">
      <img :src="imgPath+'cqu/icon-arrow.png'" alt="">
      <span>{{cqu_list.weeklyVisitCount}}</span>
    </div>
    <div class="index-data-list-pre">上周访问量 <span>{{cqu_list.previousWeeklyVisitCount}}</span></div>
  </div>
  <div class="index-data-list index-data-list3">
    <h5>学院访问排行</h5>
    <div id="cquIndexCollgeRank" style="width:100%;height:100%"></div>
  </div>
  <div class="index-data-list index-data-list4">
    <h5>本周新增资源</h5>
    <div class="index-data-list-cur">
      <img :src="imgPath+'cqu/icon-database.png'" alt="">
      <span>{{cqu_list.weeklyResourceIncreaseCount}}</span>
    </div>
    <div class="index-data-list-pre">总量 <span>{{cqu_list.totalResourceCount}}</span></div>
  </div>
  <div class="index-data-list index-data-list5">
    <h5>总资源学科分布</h5>
    <div id="cquIndexDisciplineDistribution" style="width:100%;height:100%"></div>
  </div>

  <div class="temp-loading" v-if="request_of"></div><!--加载中-->
</div></div>`;

  var list = document.getElementsByClassName('cqu_lib_data_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_lib_data_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      var template_temp_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        template_temp_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }

      new Vue({
        el: '#' + list[i].lastChild.id,
        template: template_html,
        data() {
          return {
            imgPath: window.localStorage.getItem('fileUrl') + '/public/image/',//公共图片路径
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
            request_of:true,//请求中
            cqu_list: [],
          }
        },
        mounted() {
          var template_temp_data_list = [];
          if (template_temp_set_list) {
            for (var i = 0; i < template_temp_set_list.length; i++) {
              var topCount = template_temp_set_list[i].topCount;
              var columnid = template_temp_set_list[i].id;
              var OrderRule = template_temp_set_list[i].sortType;
              template_temp_data_list.push({ count: topCount, columnId: columnid, sortField: OrderRule });
            }
          }
          this.cqu_initData(template_temp_data_list);
        },
        methods: {
          cqu_setNumber(val) {
            //数字千分位逗号分割
            let number = (val.toString().indexOf('.') !== -1) ? val.toLocaleString() : val.toString().replace(/(\d)(?=(?:\d{3})+$)/g, '$1,');
            return number;
          },
          cqu_linkTo(url) {
            window.open(url);
          },
          cqu_initData() {
            var post_url = this.post_url_vip + '/loganalysis/api/log-analysis/index-statistics';
            axios({
              url: post_url,
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                let data = res.data.data;
                let datalist = [
                  'weeklyVisitUserCount',
                  'previousWeeklyVisitUserCount',
                  'weeklyVisitCount',
                  'previousWeeklyVisitCount',
                  'weeklyResourceIncreaseCount',
                  'totalResourceCount'
                ]
                datalist.forEach(item => {
                  data[item] = this.cqu_setNumber(data[item]);
                })
                this.cqu_list = data || {};
                this.cqu_drawColumnChart();
                this.cqu_drawColumnChartDeu();
              }
              this.request_of = false;
            }).catch(err => {
              this.request_of = false;
              console.log(err);
            });
          },
          // 学院访问排行echars
          cqu_drawColumnChart() {
            let chartDom = document.getElementById('cquIndexCollgeRank');
            let myChart = echarts.init(chartDom);
            let list = []

            for (let index = this.cqu_list.departmentRankList.length - 1; index >= 0; index--) {
              const element = this.cqu_list.departmentRankList[index];
              let curlist = [];
              curlist.push(element.name);
              curlist.push(element.value);
              list.push(curlist);
            }
            console.log(list);
            let option = {
              // color:['#fd7f55','#fac858','#91cc75','#738edc'],
              dataset: [{
                source: [
                  ['name', 'value'],
                  ...list
                ]
              },
              {
                transform: {
                  type: 'sort',
                  config: { dimension: 'value', order: 'asc' }
                }
              }],
              tooltip: {
                trigger: 'axis',
                axisPointer: {
                  type: 'shadow'
                }
              },
              xAxis: { type: 'value' },
              yAxis: { type: 'category' },
              grid: {
                x: '140px',
                y: 0,
                x2: '50px',
                y2: '60px'
              },
              series: [
                {
                  type: 'bar',
                  encode: {
                    // Map the "amount" column to X axis.
                    x: 'value',
                    // Map the "product" column to Y axis
                    y: 'name'
                  },
                  itemStyle: {
                    normal: {
                      //这里是重点
                      color: function (params) {
                        //注意，如果颜色太少的话，后面颜色不会自动循环，最好多定义几个颜色
                        var colorList = ['#738edc', '#738edc', '#91cc75', '#fac858', '#fd7f55',];
                        return colorList[params.dataIndex]
                      }
                    }
                  }
                }
              ],
              axisLabel: {
                // color: '#fff',
                // fontSize: 16,
                formatter: function (value, index) {
                  var value;
                  if (value >= 1000) {
                    value = value / 1000 + 'k';
                  } else if (value < 1000) {
                    value = value;
                  }
                  return value
                }
              },
            };

            option && myChart.setOption(option);
          },
          // 总资源学科分布echars
          cqu_drawColumnChartDeu() {
            let chartDom = document.getElementById('cquIndexDisciplineDistribution');
            let myChart = echarts.init(chartDom);
            let ylist = this.cqu_list.resourceDomainRank.map(item => {
              return item.name
            });
            let xlist = this.cqu_list.resourceDomainRank.map(item => {
              return item.value
            });
            let option = {
              color: ['#738edc'],
              xAxis: {
                type: 'category',
                axisLabel: { interval: 0, rotate: 30 },
                data: ylist,
              },
              yAxis: {
                type: 'value'
              },
              series: [
                {
                  data: xlist,
                  type: 'bar'
                }
              ],
              tooltip: {
                trigger: 'axis',
                axisPointer: {
                  type: 'shadow'
                }
              },
              grid: {
                x: '70px',
                y: '10px',
                x2: '10px',
                y2: '60px'
              },
              axisLabel: {
                // color: '#fff',
                // fontSize: 16,
                formatter: function (value, index) {
                  var value;
                  if (value >= 1000) {
                    value = value / 1000 + 'k';
                  } else if (value < 1000) {
                    value = value;
                  }
                  return value
                }
              },
            };

            option && myChart.setOption(option);
          },
        },
      });
    }
  }
}
cqu_lib_data_temp1()