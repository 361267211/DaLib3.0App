function literature_project_sys_temp3() {
  var literature_project_sys_temp3_html = `<div class="featured-book-list-pad">
  <div class="featured-book-list">
    <div class="top-main-title-temp"><span>{{literature_project_sys_temp3_list.columnName||'无'}}</span><span class="e-title"></span><span class="r-btn" @click="literature_project_sys_temp3_openurl(moreUrl)">查看更多 ></span></div>
    <div class="featured-book-warp clearFloat">
        <div class="col1" @click="literature_project_sys_temp3_openurl(list_one.jumpLink)" :style="{background:'#f7fbfb url('+fileUrl+'/public/image/gztj.png) no-repeat center bottom'}"><!--<img :src="fileUrl+list_one.cover">--></div>
        <div class="col2" @click="literature_project_sys_temp3_openurl(list_two.jumpLink)" :style="{background:'#284ca3 url('+fileUrl+'/public/image/xzsd.png) no-repeat center bottom'}"><!--<img :src="fileUrl+list_two.cover">--></div>
        <div class="col3">
            <div class="col4" @click="literature_project_sys_temp3_openurl(list_three.jumpLink)" :style="{background:'url('+fileUrl+'/public/image/xzzc.png) no-repeat center bottom'}"><!--<img :src="fileUrl+list_three.cover">--></div>
            <div class="col5" @click="literature_project_sys_temp3_openurl(list_four.jumpLink)" :style="{background:'url('+fileUrl+'/public/image/xjak.png) no-repeat center bottom'}"><!--<img :src="fileUrl+list_four.cover">--></div>
        </div>
    </div>
  </div>
</div><!--特色书单-->`;

  var list = document.getElementsByClassName('literature_project_sys_temp3');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'literature_project_sys_temp3 jl_vip_zt_vray jl_vip_zt_warp');
      var literature_project_sys_temp3_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        literature_project_sys_temp3_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: literature_project_sys_temp3_html,
        data() {
          return {
            literature_project_sys_temp3_list: {},//当前列表数据
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            // post_url_vip: 'http://192.168.21.46:8000',
            post_url_vip: window.apiDomainAndPort,
            moreUrl: '',
            list_one: {},//第一条数据
            list_two: {},//第二条数据
            list_three: {},//第三条数据
            list_four: {},//第四条数据
          }
        },
        mounted() {
          if (literature_project_sys_temp3_set_list) {
            if (literature_project_sys_temp3_set_list.length > 0) {
              var list = [];
              literature_project_sys_temp3_set_list.forEach((it, i) => {
                var topCount = '';
                var columnid = '';
                var OrderRule = '';
                if (it) {
                  topCount = it['topCount'] || 16;
                  columnid = it['id'];
                  OrderRule = it['sortType'];
                }
                list.push({ count: topCount, columnId: columnid, orderrule: OrderRule });
              });
              this.literature_project_sys_temp3_initData(list);
            }
          }
        },
        methods: {
          literature_project_sys_temp3_openurl(url) {
            window.open(url);
          },
          literature_project_sys_temp3_initData(list) {
            var post_url = this.post_url_vip + '/assembly/api/assembly/GetAssemblysInPortal';
            axios({
              url: post_url,
              method: 'post',
              data: list,
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                if (res.data.data.length > 0) {
                  this.moreUrl = res.data.data[0].moreInfoUrl||''
                  this.literature_project_sys_temp3_list = res.data.data[0] || {};
                  if (res.data.data[0] && res.data.data[0].list.length > 0) {
                    this.list_one = res.data.data[0].list[0] || {};
                    this.list_two = res.data.data[0].list[1] || {};
                    this.list_three = res.data.data[0].list[2] || {};
                    this.list_four = res.data.data[0].list[3] || {};
                    console.log(this.list_one)
                  }
                }
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
literature_project_sys_temp3()