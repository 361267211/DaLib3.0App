function apps_center_sys_temp1() {
  var apps_center_sys_temp1_html = `<div class="temp-database-warp">
<div class="main-title-temp"><span>应用中心</span><span class="e-title"></span>
    <a :class="apps_center_sys_temp1_menu_num==i?'periodical-nav-active':''" href="javascript:;" v-for="(it,i) in apps_center_sys_temp1_menu_list" @click="menuClick(i)">{{it.name}}</a>
    <span class="r-btn" @click="window.open(apps_center_sys_temp1_more_url)">查看更多 ></span>
</div>
<div class="database-content c-l">
    <div class="apps_center_sys_temp1_data-list c-l">
        <div class="data-box" v-for="i in apps_center_sys_temp1_list1" @click="apps_center_sys_temp1_openUrl(i.frontUrl)">
            <div class="d-box c-l">
                <img class="d-logo" :src="fileUrl+i.appIcon">
                <div class="data-txt">{{i.appName||'无'}}</div>
            </div>
        </div>
        <div class="temp-loading" v-if="request_of"></div><!--加载中-->
        <div class="web-empty-data" v-if="!request_of && apps_center_sys_temp1_list1 && apps_center_sys_temp1_list1.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
        </div><!--暂无数据-->
    </div>
</div>
</div>`;

  var list = document.getElementsByClassName('apps_center_sys_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'apps_center_sys_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      var apps_center_sys_temp1_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: apps_center_sys_temp1_html,
        data() {
          return {
            request_of:true,//请求中
            post_url_vip: window.apiDomainAndPort,
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            apps_center_sys_temp1_more_url: '#',//跳转更多页面地址
            apps_center_sys_temp1_menu_num: 0,//当前菜单
            apps_center_sys_temp1_list1: [],//当前列表数据
            apps_center_sys_temp1_menu_list: [],//菜单列表加数据
            // apps_center_sys_temp1_menu_list:[
            //     {title:'全部'},{title:'基本应用'},
            //     {title:'资源应用'},{title:'学术情报'},
            //     {title:'空间应用'}
            // ],
          }
        },
        mounted() {
          // if (apps_center_sys_temp1_set_list) {
          //   if (apps_center_sys_temp1_set_list.length > 0) {
          //     var list = [];
          //     apps_center_sys_temp1_set_list.forEach((it, i) => {
          //       var topCount = '';
          //       var columnid = '';
          //       var OrderRule = '';
          //       if (it) {
          //         topCount = it['topCount'] || 16;
          //         columnid = it['id'];
          //         OrderRule = it['sortType'];
          //       }
          //       list.push({ Count: topCount, columnid: columnid, OrderRule: OrderRule });
          //     });
          //     this.apps_center_sys_temp1_initData(list);
          //   }
          // }
          this.apps_center_sys_temp1_initData()
        },
        methods: {
          menuClick(index) {
            this.apps_center_sys_temp1_menu_num = index;
            this.apps_center_sys_temp1_list1 = this.apps_center_sys_temp1_menu_list[index].apps || [];
          },
          apps_center_sys_temp1_initData() {
            // var post_url = this.post_url_vip + '/appcenter/api/sceneuse/getsceneusebyidbatch';
            var get_url = this.post_url_vip + '/appcenter/api/sceneuse/getappforindex';
            axios({
              url: get_url,
              method: 'get',
              // data: list,
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              this.request_of = false;
              if (res.data && res.data.statusCode == 200) {
                this.apps_center_sys_temp1_menu_list = res.data.data.items || [];
                if (this.apps_center_sys_temp1_menu_list.length > 0) {
                  this.apps_center_sys_temp1_list1 = this.apps_center_sys_temp1_menu_list[0].apps || [];
                }
                this.apps_center_sys_temp1_more_url = res.data.data.moreUrl || '#';
              }
            }).catch(err => {
              this.request_of = false;
              console.log(err);
            });
          },
          apps_center_sys_temp1_openUrl(url) {
            window.open(url);
          },
        },
      });
    }
  }
}
apps_center_sys_temp1()

