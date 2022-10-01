function header_sys_temp1() {
	var header_sys_temp1_html = `<div class="header-warp min-width">
<div class="header-tip child_bg">
	<div class="m-width">
		<div class="Welcomes"><img :src="fileUrl+'/public/image/user-icon1.png'">欢迎来到{{header_sys_temp1_list.siteName}}</div>
		<div class="right-float">
		<input type="text" :placeholder="'站内搜索'">
		<span class="line">|</span>
		<select v-model="header_sys_temp1_language">
			<option value="zh-CN">中文</option>
			<option value="zh-HANT">繁體</option>
			<option value="en-US">English</option>
		</select>
		</div>
	</div>
	</div>
	<!-- 顶部欢迎栏 end -->
	<div class="header-nav">
	<div class="m-width">
		<div class="logo-warp" class="logo-text">
		<img :src="fileUrl+header_sys_temp1_list.siteLogo" @click="goHome()" v-if="header_sys_temp1_list.siteLogo">
		</div>
		<div class="nav-warp">
			<div class="row-one" v-if="!header_sys_temp1_isLogin()">您好，读者！请<a v-if="header_sys_temp1_list.logOn" href="javascript:;" @click="header_sys_temp1_login()">登录</a></div>
			<div class="row-one" v-if="header_sys_temp1_isLogin()"><span @click="goUser()">您好，{{userInfo.name||''}}！</span><a v-if="header_sys_temp1_list.logOn" href="javascript:;" @click="header_sys_temp1_login()">退出</a></div>
			<div class="row-two clearFloat" v-if="header_sys_temp1_list.navigationColumnList">
				<a v-for="(item,index) in header_sys_temp1_list.navigationColumnList[0]" href="javascript:;" :class="header_sys_temp1_active(item)?'active':''" @click="header_sys_temp1_openUrl(item.navigationUrl, item.isOpenNewWindow)">{{item.navigationName}}</a>
			</div>
		</div>
	</div>
	</div>
	<!-- 头部菜单信息 end -->
</div>`;

	var list = document.getElementsByClassName('header_sys_temp1');
	for (var i = 0; i < list.length; i++) {
		if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
			list[i].setAttribute('class', 'header_sys_temp1 jl_vip_zt_vray');
			new Vue({
				el: '#' + list[i].lastChild.id,
				template: header_sys_temp1_html,
				data() {
					return {
						userInfo: JSON.parse(window.localStorage.getItem('userInfo') || '{}'),
						header_sys_temp1_list: {
							mainNavigationList: [],
						},
						fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
						urlInfo:JSON.parse(localStorage.getItem('urlInfo')),
                        post_url_vip: window.apiDomainAndPort,
						header_sys_temp1_language: 'zh-CN',
					}
				},
				mounted() {
					this.header_sys_temp1_initData();
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
					//选中样式
					header_sys_temp1_active(val) {
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
					//跳转链接
					header_sys_temp1_openUrl(url, isOpenNewWindow) {
						if (url) {
							let allUrl = url.indexOf('http') != -1 ? url : 'https://' + url;
							if (isOpenNewWindow) {
								window.open(allUrl, '_blank');
							} else {
								window.location.href = allUrl;
							}
						}
					},
					header_sys_temp1_login() {
						localStorage.removeItem('token');
						localStorage.setItem('COM+', window.location.href);
						window.location.href = window.casBaseUrl+'/cas/login?service=' + encodeURIComponent(window.location.origin + window.location.pathname)+'&orgcode='+window.orgcode
					},
					header_sys_temp1_Out() {
						if (confirm('确定要退出吗') == true) {
							localStorage.clear();
							sessionStorage.clear();
							localStorage.setItem('COM+', window.location.href);
							location.href = window.casBaseUrl+'/cas/logout?service=' + encodeURIComponent(window.location);
						} else {
							return false;
						}
					},
					//是否已登录
					header_sys_temp1_isLogin() {
						var token = window.localStorage.getItem('token');
						if (token && token.length > 5) {
							return true;//已登录
						} else {
							return false;
						}
					},
					header_sys_temp1_initData() {
						var post_url = this.post_url_vip+'/scenemanage/api/header-footer/header-data?templatecode=/header_sys/temp1';
						axios({
							url: post_url,
							method: 'GET',
							headers: {
								'Content-Type': 'text/plain',
								'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
							},
						}).then(res => {
							if (res.data && res.data.statusCode == 200) {
								this.header_sys_temp1_list = res.data.data || {};
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
header_sys_temp1()




