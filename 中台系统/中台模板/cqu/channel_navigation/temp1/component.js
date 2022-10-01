function cqu_channel_navigation_temp1() {
  var template_html = `<div class="index-warp-box"><div class="index-channel-box">
  <div class="index-channel-list">
    <img :src="imgPath+'cqu/icon-tushu.png'" alt="">
    <div>
      <span class="index-channel-title index-channel-title1" @click="cqu_linkTo('/#/page?id=fa08a926-13ee-4b37-aa15-9a0034308c92')">图书频道</span>
      <span>{{cqu_setNumber(cqu_list.bookChannel)}}/册</span>
    </div>
  </div>
  <div class="index-channel-list">
    <img :src="imgPath+'cqu/icon-qikan.png'" alt="">
    <div>
      <span class="index-channel-title index-channel-title2" @click="cqu_linkTo('/#/page?id=2b2d74db-67fa-443e-aa74-e2bedee9ab09')">期刊频道</span>
      <span>{{cqu_setNumber(cqu_list.journalChannel)}}/册</span>
    </div>
  </div>
  <div class="index-channel-list">
    <img :src="imgPath+'cqu/icon-shujuku.png'" alt="">
    <div>
      <span class="index-channel-title index-channel-title3" @click="cqu_linkTo('/databaseguide/#/web_dataBaseHome2','databaseguide')">数据库导航</span>
      <span>{{cqu_setNumber(cqu_list.databaseGuide)}}/个</span>
    </div>
  </div>
  <div class="index-channel-list">
    <img :src="imgPath+'cqu/icon-zaixian.png'" alt="">
    <div>
      <span class="index-channel-title index-channel-title4" @click="cqu_alert">在线学习频道</span>
      <!-- <span>{{cqu_setNumber(cqu_list.onLineStudy)}}/个</span> -->
    </div>
  </div>
</div></div>`;

  var list = document.getElementsByClassName('cqu_channel_navigation_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_channel_navigation_temp1 jl_vip_zt_vray jl_vip_zt_warp');
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
          cqu_setNumber(val) {
            //数字千分位逗号分割
            if (val) {
              let number = (val.toString().indexOf('.') !== -1) ? val.toLocaleString() : val.toString().replace(/(\d)(?=(?:\d{3})+$)/g, '$1,');
              return number;
            } else {
              return 0;
            }
          },
          cqu_alert() {
            alert('正在建设中，敬请期待！');
          },
          cqu_linkTo(url, code) {
            if (code) {
              let info = JSON.parse(localStorage.getItem('urlInfo')).find(item => item.code == code)
              window.open(info.path + url);
            } else {
              window.open(url);
            }
          },
          cqu_initData() {
            // var post_url = 'http://192.168.21.46:7603' + '/api/ResourceCenter/GetResourceStat';
            var post_url = this.post_url_vip + '/resourcecenter/api/ResourceCenter/GetResourceStat';
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
            }).catch(err => {
              console.log(err);
            });
          },

        },
      });
    }
  }
}
cqu_channel_navigation_temp1()