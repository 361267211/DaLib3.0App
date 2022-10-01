function service_sys_temp1() {
  var service_sys_temp1_html = `<div class="temp-service-warp">
<div class="main-title-temp"><span>{{service_sys_temp1_list1.columnName||'无'}}</span><span class="e-title"></span><span class="r-btn" @click="service_sys_temp1_openurl_details(service_sys_temp1_list1.jumnpLink)">查看更多 ></span></div>
<div class="database-content c-l">
    <div class="data-list c-l">
        <div class="data-box" v-for="i in (service_sys_temp1_list1.catalogueList||[])">
            <div class="d-box c-l" @click="service_sys_temp1_openurl(i)">
                <div class="d-logo"><img :src="fileUrl+i.cover"></div>
                <div class="data-txt">{{i.title||'暂无'}}</div>
            </div>
        </div>
        <div class="temp-loading" v-if="request_of"></div><!--加载中-->
        <div class="web-empty-data" v-if="!request_of && service_sys_temp1_list1 && (service_sys_temp1_list1.catalogueList||[]).length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
        </div><!--暂无数据-->
    </div>
</div>
</div>`;

  var list = document.getElementsByClassName('service_sys_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'service_sys_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      var service_sys_temp1_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        service_sys_temp1_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: service_sys_temp1_html,
        data() {
          return {
            request_of:true,//请求中
            service_sys_temp1_list1: {},
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
          }
        },
        mounted() {
          if (service_sys_temp1_set_list && service_sys_temp1_set_list.length > 0) {
            var service_sys_temp1_set_list_content = service_sys_temp1_set_list[0];
            var topCount = service_sys_temp1_set_list_content.topCount;
            var columnid = service_sys_temp1_set_list_content.id;
            var OrderRule = service_sys_temp1_set_list_content.sortType;
            this.service_sys_temp1_initData(topCount, columnid, OrderRule);
          }
        },
        methods: {
          service_sys_temp1_openurl(val) {
            var url = '#';
            if (val.navigationType == 2) {
              url = val.externalLinks;
            } else {
              url = val.jumpLink;
            }
            console.log(url);
            if (val.isOpenNewWindow) {
              window.open(url);
            } else {
              window.location.href = url;
            }
          },
          service_sys_temp1_openurl_details(url) {
            window.open(url)
          },
          service_sys_temp1_initData(topCount, columnid, OrderRule) {
            var post_url = this.post_url_vip + '/navigation/api/navigation/pront-scenes-catalogue';
            axios({
              url: post_url,
              method: 'post',
              data: [{ count: topCount, columnId: columnid, sortField: OrderRule }],
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              this.request_of = false;
              if (res.data && res.data.statusCode == 200) {
                this.service_sys_temp1_list1 = res.data.data[0] || {};
              }
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
service_sys_temp1()