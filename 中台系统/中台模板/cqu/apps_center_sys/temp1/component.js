function cqu_apps_center_sys_temp1() {
  var template_html = `<div class="index-warp-box"><div class="index-quick-box" :style="{background: 'url('+imgPath+'cqu/app-bg.png) no-repeat'}">
  <div class="index-quick-title">
    <div>
      <span class="index-quick-title-red">快应用中心</span>
      <span></span>
    </div>
    <span class="index-quick-title-more" @click="cqu_linkTo(cqu_list.moreUrl)">更多+</span>
  </div>
  <div class="index-quick-cont">
    <div class="index-quick-nav">
      <span :class="cqu_curIndex==index?'index-quick-nav-active':''" v-for="(item,index) in cqu_list.items" :key="index" @click="cqu_curIndex=index">{{item.name}}</span>
    </div>
    <div class="index-quick-list-box c-l" v-for="(item,index) in cqu_list.items" :key="index" v-show="cqu_curIndex==index">
      <div class="index-quick-list-width" v-for="(item1,index1) in item.apps" :key="index1">
        <div class="index-quick-list" @click="cqu_linkTo(item1.frontUrl)">
          <div><img :src="fileUrl+item1.appIcon" alt=""></div>
          <span>{{item1.appName}}</span>
        </div>
      </div>
      <div class="temp-loading" v-if="request_of"></div><!--加载中-->
      <div class="web-empty-data"  v-else-if="item.apps.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
      </div><!--暂无数据-->
    </div>
  </div>
</div></div>`;

  var list = document.getElementsByClassName('cqu_apps_center_sys_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_apps_center_sys_temp1 jl_vip_zt_vray jl_vip_zt_warp');
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
            request_of: true,//请求中
            cqu_list: [],
            cqu_curIndex: 0
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
          cqu_linkTo(url) {
            if (url != '' && url != '#') {
              window.open(url);
            }
          },
          cqu_initData(list) {
            var post_url = this.post_url_vip + '/appcenter/api/sceneuse/getsceneusebyidbatch';
            axios({
              url: post_url,
              method: 'POST',
              data: list,
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.cqu_list = res.data.data || [];
              }
              this.request_of = false;
            }).catch(err => {
              this.request_of = false;
              console.log(err);
            });
          },
        },
      });
    }
  }
}
cqu_apps_center_sys_temp1()