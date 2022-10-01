function loganalysis_sys_temp1() {
  var loganalysis_sys_temp1_html = `<div class="thelatestbooksonloan-warp-pad">
  <div class="thelatestbooksonloan-warp">
    <div class="top-main-title-temp"><span>最新借出图书</span><span class="e-title"></span></div>
    <div class="thelatestbooksonloan-list">
        <div class="thelatestbooksonloan-l-row" v-for="(it,i) in loganalysis_sys_temp1_list" @click="loganalysis_sys_temp1_openurl(it.detail_url)">
            <i class="num" :class="i==0?'one':(i==1?'two':(i==2?'three':''))">{{i+1}}</i>
            <p class="title">{{it.title||'无'}}</p>
            <p class="s-title">{{it.creator}}</p>
        </div>
    </div>
  </div>
</div><!--最新借出图书-->`;

  var list = document.getElementsByClassName('loganalysis_sys_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'loganalysis_sys_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      var loganalysis_sys_temp1_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        loganalysis_sys_temp1_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: loganalysis_sys_temp1_html,
        data() {
          return {
            loganalysis_sys_temp1_list: [],//当前列表数据
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
          }
        },
        mounted() {
          if (loganalysis_sys_temp1_set_list) {
            if (loganalysis_sys_temp1_set_list.length > 0) {
              var it = loganalysis_sys_temp1_set_list[0];
              this.loganalysis_sys_temp1_initData({ Count: it['topCount'], ColumnId: it['id'], OrderRule: it['sortType'] });
            }
          }
        },
        methods: {
          loganalysis_sys_temp1_openurl(url) {
            window.open(url)
          },
          loganalysis_sys_temp1_initData(list) {
            var post_url = this.post_url_vip + '/articlerecommend/api/newlyload/getnewlyloadbook?pagesize=' + list.Count + '&pageindex=1';
            axios({
              url: post_url,
              method: 'get',
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                if (res.data.data && res.data.data.items.length > 0) {
                  this.loganalysis_sys_temp1_list = res.data.data.items || [];
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
loganalysis_sys_temp1()