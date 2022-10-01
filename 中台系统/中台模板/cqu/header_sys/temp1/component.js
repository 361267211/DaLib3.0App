function cqu_header_sys_temp1() {
  var template_html = `<div class="index-header">
  <div class="index-header-box">
  <img :src="fileUrl+cqu_list.siteLogo" @click="goHome()" v-if="cqu_list.siteLogo">
  <div class="index-header-top">
    <div v-if="!cqu_isLogin">您好，读者！请<span class="login-btn" @click="cqu_login">登录</span></div>
    <div v-if="cqu_isLogin" @click="goUser()">您好，{{cqu_userInfo.name}}</div>
    <span v-for="(item,index) in (cqu_list.navigationColumnList[1]||[])" :key="index" @click="cqu_linkTo(item.navigationUrl, item.isOpenNewWindow)">{{item.navigationName}}</span>
    <!-- <span @click="cqu_linkTo(cqu_list.personalLibrary.navigationUrl, cqu_list.personalLibrary.isOpenNewWindow)">我的图书馆</span>
    <span @click="cqu_linkTo(cqu_list.englishSite.navigationUrl, cqu_list.englishSite.isOpenNewWindow)">ENGLISH</span>
    <span @click="cqu_linkTo(cqu_list.oldSite.navigationUrl, cqu_list.oldSite.isOpenNewWindow)">旧版</span> -->
    <span v-if="cqu_isLogin" @click="cqu_loginOut">退出</span>
  </div>
  <div class="index-header-nav">
    <span v-for="(item,index) in (cqu_list.navigationColumnList[0]||[])" :key="index" @click="cqu_linkTo(item.navigationUrl, item.isOpenNewWindow)">{{item.navigationName}}</span>
  </div>
</div>
</div>`;

  var list = document.getElementsByClassName('cqu_header_sys_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_header_sys_temp1 jl_vip_zt_vray jl_vip_zt_warp');
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
            urlInfo:JSON.parse(localStorage.getItem('urlInfo')),
            post_url_vip: window.apiDomainAndPort,
            cqu_list: {navigationColumnList:[]},
            cqu_isLogin: false,
            cqu_userInfo: {},
          }
        },
        mounted() {
          if (localStorage.getItem('token')) {
            this.cqu_isLogin = true;
            this.cqu_userInfo = JSON.parse(localStorage.getItem('userInfo'))
          }
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
          //回首页
					goHome(){
						let info = this.urlInfo.find(item => item.code == 'index');
						window.location.href = info.path + '/#/index?page=1';
					},
					//到个人中心
					goUser(){
						let info = this.urlInfo.find(item => item.code == 'usermanage');
						window.location.href = info.path + '/usermanage/#/web_library';
					},
          cqu_linkTo(url, isOpenNewWindow) {
            if (url) {
							let allUrl = url.indexOf('http') != -1 ? url : 'https://' + url;
							if (isOpenNewWindow) {
								window.open(allUrl, '_blank');
							} else {
								window.location.href = allUrl;
							}
						}
          },
          cqu_login() {
            localStorage.removeItem('token');
            localStorage.setItem('COM+', window.location.href);
            window.location.href = window.casBaseUrl + '/cas/login?service=' + encodeURIComponent(window.location.origin + window.location.pathname)+'&orgcode='+window.orgcode;
          },
          cqu_loginOut() {
            localStorage.removeItem('token');
            let current = window.location.href;
            localStorage.setItem('COM+', current);
            location.href = window.casBaseUrl + '/cas/logout?service=' + encodeURIComponent(window.location);
          },
          cqu_initData() {
            var post_url = this.post_url_vip + '/scenemanage/api/header-footer/header-data?templatecode=/cqu/header_sys/temp1';
            axios({
              url: post_url,
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.cqu_list = res.data.data || {navigationColumnList:[]};
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
cqu_header_sys_temp1()