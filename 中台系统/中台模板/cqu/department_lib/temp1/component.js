function cqu_department_lib_temp1() {
  var template_html = `<div class="index-warp-box"><div class="index-numlib-box">
  <div class="index-numlib-title">
    <div>
      <span class="index-numlib-title-red">学院数字图书馆</span>
      <span></span>
    </div>
  </div>
  <div class="index-numlib-cont">
    <div class="index-numlib-list" v-for="(item,index) in cqu_list" :key="index">
      <h5>{{item.categoryName}}</h5>
      <div>
        <span @click="cqu_linkTo(items.url)" :href="items.url" v-for="(items,indexs) in item.sdeptServiceItems" :key="'x'+indexs">{{items.userGroupName}}</span>
      </div>
    </div>

    <div class="temp-loading" v-if="request_of"></div><!--加载中-->
    <div class="web-empty-data"  v-if="!request_of&&cqu_list.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
    </div><!--暂无数据-->
  </div>
</div></div>`;

  var list = document.getElementsByClassName('cqu_department_lib_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_department_lib_temp1 jl_vip_zt_vray jl_vip_zt_warp');
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
            request_of:true,//请求中
            cqu_list: [],
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
            window.open(url);
          },
          cqu_initData() {
            var post_url = this.post_url_vip + '/departmentlib/api/department-liba/all-department-lib';
            axios({
              url: post_url,
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
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
cqu_department_lib_temp1()