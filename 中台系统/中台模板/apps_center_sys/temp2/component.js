function apps_center_sys_temp2() {
  var apps_center_sys_temp2_html = `<div class="temp-home1-warp">
<div class="hot-apps">
    <div class="main-title-temp"><span>推荐应用</span><span class="e-title">/ College library</span></div>
    <div class="apps-conent c-l">
        <div class="box" v-for="i in apps_center_sys_temp2_list1" @click="apps_center_sys_temp2_openUrl(i.frontUrl)">
            <img :src="i.appIcon" class="apps-c-logo">
            <span :title="i.appName">{{i.appName||'无'}}</span>
        </div>
        <div class="temp-loading" v-if="request_of"></div><!--加载中-->
        <div class="web-empty-data" v-if="!request_of && apps_center_sys_temp2_list1 && apps_center_sys_temp2_list1.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
        </div><!--暂无数据-->
    </div>
</div>
</div>`;


  var list = document.getElementsByClassName('apps_center_sys_temp2');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'apps_center_sys_temp2 jl_vip_zt_vray jl_vip_zt_warp');
      var apps_center_sys_temp2_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: apps_center_sys_temp2_html,
        data() {
          return {
            request_of:true,//请求中
            apps_center_sys_temp2_list1: [],
            post_url_vip: window.apiDomainAndPort,
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
          }
        },
        mounted() {
          var set_content = apps_center_sys_temp2_set_list[0];
          var topCount = '';
          var columnid = '';
          var OrderRule = '';
          if (set_content) {
            topCount = set_content['topCount'] || 16;
            columnid = set_content['id'];
            OrderRule = set_content['sortType'];
          }
          this.apps_center_sys_temp2_initData(topCount, columnid, OrderRule);
        },
        methods: {
          apps_center_sys_temp2_initData(topCount, columnid, OrderRule) {
            var post_url = this.post_url_vip + '/appcenter/api/sceneuse/getsceneusebyid';
            if (topCount) {
              post_url = post_url + '?Count=' + topCount;
            }
            if (columnid) {
              post_url = post_url + '&columnid=' + columnid;
            }
            if (OrderRule) {
              post_url = post_url + '&OrderRule=' + OrderRule;
            }
            axios({
              url: post_url,
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              this.request_of = false;
              if (res.data && res.data.statusCode == 200) {
                this.apps_center_sys_temp2_list1 = res.data.data.apps || [];
              }
            }).catch(err => {
              this.request_of = false;
              console.log(err);
            });
          },
          apps_center_sys_temp2_openUrl(url) {
            window.open(url);
          },
        },
      });
    }
  }
}
apps_center_sys_temp2()