function cqu_navigation_sys_temp1() {
  var template_html = `<div class="index-warp-box">
    <div class="index-navgetion-box">
        <div class="index-navgetion-img" v-if="cqu_list.catalogueList&&cqu_list.catalogueList.length>0">
          <div v-for="(item,index) in cqu_list.catalogueList" :key="index" @click="cqu_linkTo(item)">
            <img :src="fileUrl+item.cover" alt="">
          </div>
        </div>
        <div class="temp-loading" v-if="request_of"></div><!--加载中-->
        <div class="web-empty-data"  v-else-if="!cqu_list.catalogueList||cqu_list.catalogueList.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
        </div><!--暂无数据-->
      </div>
    </div>
  </div>`;

  var list = document.getElementsByClassName('cqu_navigation_sys_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_navigation_sys_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      var template_temp_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        template_temp_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }

      new Vue({
        el: '#' + list[i].lastChild.id,
        template: template_html,
        data() {
          return {
            request_of: true,
            imgPath: window.localStorage.getItem('fileUrl') + '/public/image/',//公共图片路径
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
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
          cqu_linkTo(val) {
            var url = '#';
            if (val.navigationType == 2) {
              url = val.externalLinks;
            } else {
              url = val.jumpLink;
            }
            if (val.isOpenNewWindow) {
              window.open(url);
            } else {
              window.location.href = url;
            }
          },
          cqu_initData(list) {
            var post_url = this.post_url_vip + '/navigation/api/navigation/pront-scenes-catalogue';
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
                this.cqu_list = res.data.data[0] || [];
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
cqu_navigation_sys_temp1()