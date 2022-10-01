function database_nav_sys_temp2() {
  var database_nav_sys_temp2_html = `<div class="tmp-detail">
  <div class="title-box">
    <span class="left">常用数据库</span>
    <span class="right">
      <span @click="linkTo(databaseMoreUrl)">更多 </span>
    </span>
  </div>
  <div class="tem-content">
    <p class="database-item" v-for="item in database_nav_sys_temp2_list" :key="item" @click="database_nav_sys_temp2_openurlDetails(item.directUrls[0].url,item.id)" v-if="!request_of">
      <span>{{item.title}}</span>
      <img class="data-icon" :src="fileUrl+'/public/image/icon_focus.png'" alt="" v-if="item.type==1">
      <img class="data-icon" :src="fileUrl+'/public/image/icon_hot.png'" alt="" v-else>
    </p>
    <!--加载中-->
    <div class="temp-loading" v-if="request_of"></div>
    <!--暂无数据-->
    <div class="web-empty-data"  v-else-if="database_nav_sys_temp2_list.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
    </div>
  </div>
</div>`;


  var list = document.getElementsByClassName('database_nav_sys_temp2');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'database_nav_sys_temp2 jl_vip_zt_vray jl_vip_zt_warp');
      // var database_nav_sys_temp2_set_list = list[i].dataset.set ? JSON.parse(list[i].dataset.set.replace(/'/g, '"')) : [];
      var database_nav_sys_temp2_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        database_nav_sys_temp2_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: database_nav_sys_temp2_html,
        data() {
          return {
            request_of: true,
            post_url_vip: window.apiDomainAndPort,
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            database_nav_sys_temp2_list: [],
            databaseMoreUrl: ''
          }
        },
        mounted() {
          // console.log(database_nav_sys_temp2_set_list);
          // if (database_nav_sys_temp2_set_list) {
          //   for (var i = 0; i < database_nav_sys_temp2_set_list.length; i++) {
          //     var topCount = database_nav_sys_temp2_set_list[i].topCount;
          //     var columnid = database_nav_sys_temp2_set_list[i].id;
          //     var database_nav_sys_temp2_data_list = [{ count: topCount, columnId: columnid }];
          //     this.database_nav_sys_temp2_initData(database_nav_sys_temp2_data_list);
          //   }
          // }
          this.database_nav_sys_temp2_initData();
        },
        methods: {
          linkTo(url) {
            window.open(url)
          },
          //查看详情，到详情页面
          database_nav_sys_temp2_openurlDetails(url, id) {
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
            window.open(url)
          },
          database_nav_sys_temp2_initData() {
            var post_url = this.post_url_vip + '/databaseguide/api/database-terrace/my-favorite-databases/12';
            axios({
              url: post_url,
              method: 'GET',
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.databaseMoreUrl = res.data.data.moreUrl;
                this.database_nav_sys_temp2_list = res.data.data.databases;
              }
              this.request_of = false;
            }).catch(err => {
              this.request_of = false;
            });
          },
        },
      });
    }
  }
}
database_nav_sys_temp2()