function cqu_literature_recommend_sys_temp2() {
  var template_html = `<div class="index-warp-box"><div class="index-ranking-box">
  <div class="index-ranking-title">
    <div>
      <span @click="cqu_active=index" :class="index==cqu_active?'index-ranking-title-red':''" v-for="(item,index) in cqu_list" :key="index">{{item.name || '无'}}</span>
    </div>
    <span class="index-ranking-title-more" @click="cqu_linkTo(cqu_list[cqu_active].moreUrl)">更多+</span>
  </div>
  <div class="index-ranking-cont" v-show="cqu_active==index" v-for="(item,index) in cqu_list" :key="index">
    <template v-if="item.titleInfos.length>0">
      <div class="index-ranking-list-width" v-for="(item1,index1) in item.titleInfos" :key="index1" >
        <div class="index-ranking-list" @click="cqu_linkTo(item1.detail_url)">
          <img :src="item1.cover||(imgPath+'default-cover-dissertation.jpg')" :onerror="default_img">
          <span class="book-name" v-if="!item1.cover||item1.cover==''">{{item1.title}}</span>
          <div>{{item1.title}}</div>
        </div>
      </div>
    </template>
    <div class="web-empty-data"  v-if="item.titleInfos.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
    </div><!--暂无数据-->
  </div>
  <div class="index-ranking-cont" v-if="request_of"><div class="temp-loading"></div><!--加载中--></div>
</div></div>`;

  var list = document.getElementsByClassName('cqu_literature_recommend_sys_temp2');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_literature_recommend_sys_temp2 jl_vip_zt_vray jl_vip_zt_warp');
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
            post_url_vip: window.apiDomainAndPort,
            request_of:true,//请求中
            cqu_list: [],
            default_img: 'this.src="'+window.localStorage.getItem('fileUrl')+'/public/image/img-error-174x241.jpg"',
            cqu_active: 0,
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
            axios({
              url: post_url,
              method: 'POST',
              data:list,
              headers: {
                'Content-Type': 'application/json',
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
cqu_literature_recommend_sys_temp2()