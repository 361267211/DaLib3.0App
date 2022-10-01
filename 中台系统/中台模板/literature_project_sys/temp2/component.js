function literature_project_sys_temp2() {
  var literature_project_sys_temp2_html = `<div class="tmp-detail">
  <div class="title-box">
    <span class="left">我的专题</span>
    <span class="right">
      <span @click="literature_project_temp2_openurlDetails(myAssemblysMoreUrl)">更多 </span>
    </span>
  </div>
  <div class="tem-content">
    <div class="topic-item" v-for="item in literature_project_sys_temp2_list" :key="item" @click="literature_project_temp2_openurlDetails(item.jumpLink)" v-if="!request_of">
      <img :src="fileUrl+item.cover" alt="">
      <p>{{item.assemblyName}}</p>
    </div>
    <!--加载中-->
    <div class="temp-loading" v-if="request_of"></div>
    <!--暂无数据-->
    <div class="web-empty-data"  v-else-if="literature_project_sys_temp2_list.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
    </div>
  </div>
</div>`;

  var list = document.getElementsByClassName('literature_project_sys_temp2');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'literature_project_sys_temp2 jl_vip_zt_vray jl_vip_zt_warp');
      var literature_project_sys_temp2_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        literature_project_sys_temp2_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: literature_project_sys_temp2_html,
        data() {
          return {
            request_of: true,
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
            literature_project_sys_temp2_list: [],
            myAssemblysMoreUrl:''
          }
        },
        mounted() {
          // if (literature_project_sys_temp2_set_list) {
          //   var topCount = literature_project_sys_temp2_set_list[0].topCount;
          //   var columnid = literature_project_sys_temp2_set_list[0].id;
          //   var sortType = literature_project_sys_temp2_set_list[0].sortType;
          //   var literature_project_sys_temp2_data_list = [{ count: topCount, columnId: columnid, orderrule: sortType }];
          //   this.literature_project_sys_temp2_initData(literature_project_sys_temp2_data_list);
          // }
          this.literature_project_sys_temp2_initData();
        },
        methods: {
          //查看详情，到详情页面
          literature_project_temp2_openurlDetails(url) {
            window.open(url)
          },
          literature_project_sys_temp2_initData() {
            var post_url = this.post_url_vip + '/assembly/api/Assembly/GetMyLibraryAssembly?count=6';
            axios({
              url: post_url,
              method: 'GET',
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.myAssemblysMoreUrl = res.data.data.moreUrl;
                this.literature_project_sys_temp2_list = res.data.data.myLibraryAssemblys || [];
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
literature_project_sys_temp2()