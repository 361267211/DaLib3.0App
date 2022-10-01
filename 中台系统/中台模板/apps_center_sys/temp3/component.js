function apps_center_sys_temp3() {
  var apps_center_sys_temp3_html = `<div class="tmp-detail">
  <div class="title-box">
    <span class="left">通知消息</span>
    <span class="right">
      <!-- <span @click="apps_center_sys_temp3_openurlDetails(apps_center_sys_temp3_list.linkUrl)">更多 </span> -->
    </span>
  </div>
  <div class="tem-content">
    <p class="info-item" v-for="item in apps_center_sys_temp3_list.noticeMessages" :key="item" v-if="!request_of">
      <span>【{{item.appName}}】{{item.intro}}</span>
      <span>{{timeFormat(item.sendTime)}}</span>
    </p>
    <!--加载中-->
    <div class="temp-loading" v-if="request_of"></div>
    <!--暂无数据-->
    <div class="web-empty-data"  v-else-if="apps_center_sys_temp3_list.noticeMessages.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
    </div>
  </div>
</div>`;


  var list = document.getElementsByClassName('apps_center_sys_temp3');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'apps_center_sys_temp3 jl_vip_zt_vray jl_vip_zt_warp');
      var apps_center_sys_temp3_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        apps_center_sys_temp3_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: apps_center_sys_temp3_html,
        data() {
          return {
            request_of: true,
            apps_center_sys_temp3_list: {},
            post_url_vip: window.apiDomainAndPort,
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
          }
        },
        mounted() {
          if (apps_center_sys_temp3_set_list) {
            for (var i = 0; i < apps_center_sys_temp3_set_list.length; i++) {
              var topCount = apps_center_sys_temp3_set_list[i].topCount;
              var OrderRule = apps_center_sys_temp3_set_list[i].sortType;
              this.apps_center_sys_temp3_initData(topCount, OrderRule);
            }
          }
        },
        methods: {
          apps_center_sys_temp3_openurlDetails(url) {
            window.open(url)
          },
          apps_center_sys_temp3_initData(topCount, OrderRule) {
            var post_url = this.post_url_vip + '/noticecenter/api/reader-message/notice-center-scene-message';
            if (topCount) {
              post_url = post_url + '?TopCount=' + topCount;
            }
            if (OrderRule) {
              post_url = post_url + '&SortType=' + OrderRule;
            }
            axios({
              url: post_url,
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
                "Access-Control-Allow-Origin": '*',
                'Access-Control-Allow-Method': '*'
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.apps_center_sys_temp3_list = res.data.data;
              }
              this.request_of = false;
            }).catch(err => {
              this.request_of = false;
              console.log(err);
            });
          },
          timeFormat(date, time = '分') {
            let times = {
              '年': 'YYYY',
              '月': 'YYYY-mm',
              '日': 'YYYY-mm-dd',
              '时': 'YYYY-mm-dd HH',
              '分': 'YYYY-mm-dd HH:MM',
              '秒': 'YYYY-mm-dd HH:MM:SS',
            }
            let format = times[time];
            date = new Date(date);
            const dataItem = {
              'Y+': date.getFullYear().toString(),
              'm+': (date.getMonth() + 1).toString(),
              'd+': date.getDate().toString(),
              'H+': date.getHours().toString(),
              'M+': date.getMinutes().toString(),
              'S+': date.getSeconds().toString(),
            };
            Object.keys(dataItem).forEach((item) => {
              const ret = new RegExp(`(${item})`).exec(format);
              if (ret) {
                format = format.replace(ret[1], ret[1].length === 1 ? dataItem[item] : dataItem[item].padStart(ret[1].length, '0'));
              }
            });
            return format;
          }
        },
      });
    }
  }
}
apps_center_sys_temp3()