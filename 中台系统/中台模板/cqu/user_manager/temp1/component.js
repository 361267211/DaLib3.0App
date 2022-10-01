function cqu_user_manager_temp1() {
  var template_html = `<div class="index-warp-box"><div class="index-my-box">
  <div class="index-my-read" v-if="cqu_isLogin">
    <div class="index-my-title">我的阅读信息</div>
    <div class="index-my-read-cont" v-if="!request_of">
      <div v-for="(item,index) in cqu_myInfo" :key="index">
        <span @click="cqu_linkTo(item.link)">{{item.count || 0}}</span>
        <span>{{item.title}}</span>
      </div>
    </div>
    <div v-else>
      <div class="temp-loading" v-if="request_of"></div><!--加载中-->
      <div class="web-empty-data"  v-else :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
      </div><!--暂无数据-->
    </div>
  </div>
  <div class="index-my-database" v-if="cqu_isLogin">
    <div class="index-my-title">我的常用数据库<span @click="cqu_linkTo(cqu_databaseMoreUrl)">更多+</span></div>
    <div class="index-my-database-cont">
      <div v-for="(item,index) in cqu_databaseList" :key="index" @click="cqu_databaselinkTo(item.directUrls[0].url,item.id)">
        <span class="database-cont-gretag" v-if="item.type==1">藏</span>
        <span class="database-cont-redtag" v-else>热</span>
        <span class="database-cont-title">{{item.title}}</span>
      </div>
    </div>
  </div>
  <div class="index-nologin-tips" v-if="!cqu_isLogin" :style="{'background-image': 'url('+fileUrl+'/public/image/cqu_no-login-bg2.png),url('+fileUrl+'/public/image/cqu_no-login-bg1.png)'}">
    <div class="index-nologin-div-warp">
      <span>Hi！您好<br>
      请登录您的专属图书馆</span>
      <div class="index-login-btn" @click="cqu_login">点击登录</div>
    </div>
  </div>
  <div class="index-my-recommd">
    <div class="index-my-title">推荐应用<span @click="cqu_linkTo(cqu_appMoreUrl)">更多+</span></div>
    <div class="index-my-recommd-cont">
      <div v-for="(item,index) in cqu_appList" :key="index" @click="cqu_linkTo(item.frontUrl)" v-if="index<12">
        <div>
          <img :src="fileUrl+item.appIcon" alt="">
        </div>
        <span>{{item.appName}}</span>
      </div>
    </div>
  </div>
</div></div>`;

  var list = document.getElementsByClassName('cqu_user_manager_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_user_manager_temp1 jl_vip_zt_vray jl_vip_zt_warp');
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
            cqu_myInfo: {},
            cqu_appList: [],
            cqu_appMoreUrl: '',
            cqu_databaseList: [],
            cqu_databaseMoreUrl: '',
            cqu_isLogin: false,
          }
        },
        mounted() {
          var template_temp_data_list = [];
          // for (var i = 0; i < template_temp_set_list.length; i++) {
          //   var topCount = template_temp_set_list[i].topCount;
          //   var columnid = template_temp_set_list[i].id;
          //   var OrderRule = template_temp_set_list[i].sortType;
          //   template_temp_data_list.push({ count: topCount, columnId: columnid, sortField: OrderRule });
          // }
          this.cqu_isLogin = localStorage.getItem('token') ? true : false;
          this.cqu_initData(template_temp_data_list);
        },
        methods: {
          cqu_login() {
            localStorage.removeItem('token');
            localStorage.setItem('COM+', window.location.href);
            window.location.href = window.casBaseUrl + '/cas/login?service=' + encodeURIComponent(window.location.origin + window.location.pathname) + '&orgcode=' + window.orgcode
          },
          cqu_linkTo(url) {
            if (url != '' && url != '#') {
              window.open(url);
            }
          },
          cqu_databaselinkTo(url, id) {
            axios({
              url: this.post_url_vip + '/databaseguide/api/database-terrace/visit-databases?databaseid=' + id,
              method: 'GET',
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {

            }).catch(err => {
              console.log(err);
            });
            window.open(url);
          },
          cqu_initData() {
            if (this.cqu_isLogin) {
              //   获取阅读信息
              axios({
                url: this.post_url_vip + '/usermanage/api/scene/statis-items',
                method: 'POST',
                data: {
                  // "userId": "",
                  // "userKey": "",
                  // "sortField": "",
                  // "sortType": ""
                },
                headers: {
                  'Content-Type': 'application/json',
                  'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
                },
              }).then(res => {
                if (res.data && res.data.statusCode == 200) {
                  this.cqu_myInfo = res.data.data || [];
                }
                this.request_of = false;
              }).catch(err => {
                console.log(err);
                this.request_of = false;
              });
              // 获取我的数据库 
              axios({
                url: this.post_url_vip + '/databaseguide/api/database-terrace/my-favorite-databases/8',
                method: 'GET',
                headers: {
                  'Content-Type': 'text/plain',
                  'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
                },
              }).then(res => {
                if (res.data && res.data.statusCode == 200) {
                  this.cqu_databaseMoreUrl = res.data.data.moreUrl;
                  this.cqu_databaseList = res.data.data.databases || [];
                }
              }).catch(err => {
                console.log(err);
              });
            }
            //   获取推荐应用
            axios({
              url: this.post_url_vip + '/appcenter/api/userapplication/getrecommendappmore',
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.cqu_appMoreUrl = res.data.data.moreUrl;
                this.cqu_appList = res.data.data.recommendApps || [];
              }
            }).catch(err => {
              console.log(err);
            });
          },
        },
      });
    }
  }
}
cqu_user_manager_temp1()