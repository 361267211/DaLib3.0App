function cqu_literature_recommend_sys_temp1() {
  var template_html = `<div class="index-warp-box"><div class="index-newbook-box">
  <div class="index-newbook-title">
    <div>
      <span class="index-newbook-title-red">{{cqu_list.name || '无'}}</span>
      <!-- <span>NEW BOOK EXPRESS</span> -->
    </div>
    <span class="index-newbook-title-more" @click="cqu_linkTo(cqu_list.moreUrl)">更多+</span>
  </div>
  <div class="index-newbook-cont c-l" v-if="!request_of">
    <div class="index-newbook-list-width" v-for="(item,index) in cqu_list.titleInfos" :key="index">
      <div class="index-newbook-list" @click="cqu_linkTo(item.detail_url)">
        <img :src="item.cover" :onerror="default_img">
        <div>{{item.title}}</div>
      </div>
    </div>
  </div>
  <div class="index-newbook-cont" v-else>
    <div class="temp-loading" v-if="request_of"></div><!--加载中-->
    <div class="web-empty-data"  v-else-if="cqu_list&&cqu_list.titleInfos.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
    </div><!--暂无数据-->
  </div>
</div></div>`;

  var list = document.getElementsByClassName('cqu_literature_recommend_sys_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_literature_recommend_sys_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      var template_temp_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        template_temp_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }

      new Vue({
        el: '#' + list[i].lastChild.id,
        template: template_html,
        data() {
          return {
            imgPath: window.localStorage.getItem('fileUrl')+'/public/image/',//公共图片路径
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            default_img: 'this.src="'+window.localStorage.getItem('fileUrl')+'/public/image/img-error-174x241.jpg"',
            post_url_vip: window.apiDomainAndPort,
            request_of:true,//请求中
            cqu_list: [],
          }
        },
        mounted() {
          var template_temp_data_list = [];
          if(template_temp_set_list){
            for (var i = 0; i < template_temp_set_list.length; i++) {
              var topCount = template_temp_set_list[i].topCount;
              var columnid = template_temp_set_list[i].id;
              var OrderRule = template_temp_set_list[i].sortType;
              template_temp_data_list.push({ count: topCount, columnId: columnid, OrderRule: OrderRule });
            }
          }
          this.cqu_initData(template_temp_data_list);
        },
        methods: {
          cqu_linkTo(url) {
            window.open(url);
          },
          cqu_initData(list) {
            var post_url = this.post_url_vip + '/articlerecommend/api/sceneuse/getsceneusebyidbatch';
            // var post_url = this.post_url_vip + '/opac/api/opac/recommend-new-book';//改为文献推荐
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
cqu_literature_recommend_sys_temp1()