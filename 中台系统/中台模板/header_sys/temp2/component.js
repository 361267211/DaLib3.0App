"use strict";
function header_sys_temp2() {
	var header_sys_temp2_html = `<div class="header-temp2-page">
<div class="header-tip child_bg">
<div class="m-width" v-if="!header_sys_temp2_isLogin()">您好，读者！请<a v-if="header_sys_temp2_list.logOn" href="javascript:;" @click="header_sys_temp2_login()">登录</a></div>
<div class="m-width" v-if="header_sys_temp2_isLogin()"><span @click="goUser()">您好，{{userInfo.name||''}}！</span><a v-if="header_sys_temp2_list.logOn" href="javascript:;" @click="header_sys_temp2_Out()">退出</a></div>
</div>
<div class="header-nav clearFloat">
  <div class="m-width">
	<div class="left"><img :src="fileUrl+header_sys_temp2_list.siteLogo" @click="goHome()" v-if="header_sys_temp2_list.siteLogo"></div>
	  <div class="right" v-if="header_sys_temp2_list.navigationColumnList">
		<a class="col clearFloat" v-for="(item,index) in header_sys_temp2_list.navigationColumnList[0]" :key="index" :class="header_sys_temp2_active(item)?'active':''" href="javascript:;" @click="header_sys_temp2_openUrl(item.navigationUrl, item.isOpenNewWindow)">
		  <img :src="fileUrl+item.navigationIcon" v-if="item.navigationIcon">
		  <span class="child_color_text_hover">{{item.navigationName}}</span>
		</a>
	  </div>
  </div>
</div>
</div>`;


	var list = document.getElementsByClassName('header_sys_temp2');
	for (var i = 0; i < list.length; i++) {
		if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
			list[i].setAttribute('class', 'header_sys_temp2 jl_vip_zt_vray');
			new Vue({
				el: '#' + list[i].lastChild.id,
				template: header_sys_temp2_html,
				data() {
					return {
						userInfo: JSON.parse(window.localStorage.getItem('userInfo') || '{}'),
						fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
						urlInfo:JSON.parse(localStorage.getItem('urlInfo')),
                        post_url_vip: window.apiDomainAndPort,
						header_sys_temp2_list: {
							mainNavigationList: [],
						},
					}
				},
				mounted() {
					this.header_sys_temp2_initData();
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
					//跳转链接
					header_sys_temp2_openUrl(url, isOpenNewWindow) {
						if (url) {
							let allUrl = url.indexOf('http') != -1 ? url : 'https://' + url;
							if (isOpenNewWindow) {
								window.open(allUrl, '_blank');
							} else {
								window.location.href = allUrl;
							}
						}
					},
					header_sys_temp2_login() {
						localStorage.removeItem('token');
						localStorage.setItem('COM+', window.location.href);
						window.location.href = window.casBaseUrl+'/cas/login?service=' + encodeURIComponent(window.location.origin + window.location.pathname)+'&orgcode='+window.orgcode
					},
					header_sys_temp2_Out() {
						if (confirm('确定要退出吗') == true) {
							localStorage.clear();
							sessionStorage.clear();
							localStorage.setItem('COM+', window.location.href);
							location.href = window.casBaseUrl+'/cas/logout?service=' + encodeURIComponent(window.location);
						} else {
							return false;
						}
					},
					//选中样式
					header_sys_temp2_active(val) {
						var url_hash = (location.hash||'1').length-1;
						if (val.navigationUrl.indexOf((location.hash.slice(0,url_hash)||'*'))>-1) {
							return true;
						} else {
							if(val.navigationUrl == location.origin && location.hash=='#/index'){
								return true;
							}else{
								return false;
							}
						}
					},
					//是否已登录
					header_sys_temp2_isLogin() {
						var token = window.localStorage.getItem('token');
						if (token && token.length > 5) {
							return true;//已登录
						} else {
							return false;
						}
					},
					header_sys_temp2_initData() {
						var post_url = this.post_url_vip+'/scenemanage/api/header-footer/header-data?templatecode=/header_sys/temp2';
						axios({
							url: post_url,
							method: 'GET',
							headers: {
								'Content-Type': 'text/plain',
								'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
							},
						}).then(res => {
							if (res.data && res.data.statusCode == 200) {
								this.header_sys_temp2_list = res.data.data || {};
							}
						}).catch(err => {
							console.log(err);
						});
					},
				}
			});
		}
	}
}
header_sys_temp2()


