function literature_recommend_sys_temp2() {
    var literature_recommend_sys_temp2_html = `
<div class="temp-newbook-warp">
        <div class="main-title-temp"><span>新书速递</span><span class="e-title"></span></div>
        <div class="newbook-content c-l">
        <!--<div class="newbook-c-left">
                <div class="book-img">
                    <div class="book-score">
                        <span>5.0</span>
                        <img src="http://192.168.21.71:9000/image/xing.png">
                        <img src="http://192.168.21.71:9000/image/xing.png">
                        <img src="http://192.168.21.71:9000/image/xing.png">
                        <img src="http://192.168.21.71:9000/image/xing.png">
                    </div>
                    <img v-if="literature_recommend_sys_temp2_allList_fist&&literature_recommend_sys_temp2_allList_fist[0]" :src="literature_recommend_sys_temp2_allList_fist[0].cover||'http://192.168.21.71:9000/image/book2.jpg'" class="book-img">
                </div>
                <div class="book-info">
                    <span class="b-i-title" v-if="literature_recommend_sys_temp2_allList_fist&&literature_recommend_sys_temp2_allList_fist[0]">{{literature_recommend_sys_temp2_allList_fist[0].title||''}}</span>
                    <span class="b-i-author" v-if="literature_recommend_sys_temp2_allList_fist&&literature_recommend_sys_temp2_allList_fist[0]">{{literature_recommend_sys_temp2_allList_fist[0].creator}}</span>
                    <span class="b-i-info" v-if="literature_recommend_sys_temp2_allList_fist&&literature_recommend_sys_temp2_allList_fist[0]">{{literature_recommend_sys_temp2_allList_fist[0].description}}</span>
                </div>
            </div>left end-->

            <div class="newbook-c-center c-l">
                <div class="temp-loading" v-if="request_of"></div><!--加载中-->
                <div class="web-empty-data" v-if="!request_of && literature_recommend_sys_temp2_allList_fist && literature_recommend_sys_temp2_allList_fist.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
                </div><!--暂无数据-->
                <div class="n-c-box" v-for="(it,i) in literature_recommend_sys_temp2_allList_fist" @click="literature_recommend_sys_temp2_openurl(it.detail_url)" v-if="i!=0">
                    <img :src="it.cover" :onerror="default_img">
                    <span class="bok-name">{{it.title}}</span>
                    <span class="box-author">{{it.creator}}</span>
                </div>
            </div><!--center end-->

            <div class="newbook-c-right">
                <div class="n-c-r-title">
                    <img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAWCAYAAADAQbwGAAACu0lEQVQ4T6VVO2hUQRQ9d2YgAS0sLFKssIEUFoIWCUZUjGAKq1gqKLtLEkQUosSgFmKKFAlRrESL6D4/YEqFgIVCFAQFLSyELRRMkYBgwBQRhMydI/Nedt24+Wicbt7cOfece+59I9hgDQMub8xpiHQWVU9tFC/rBZSBFnHuGYA9IB/9F2AZaBbn3gHYFZMKMFrw/so/M4wShwGfGDMIY67XAYwUvb9a3ceExpiBLyHciPHV7yskPwD20piuQghjibWfINJWAyQni6on6hkm1j4EkP+hevQssLis5HdI4lwF5GuqDolz31fII+epuqME/Kx+T5wbB3AR5MsZ1e7ItMawDHSJc9MgP1O1W5z70lAvMqHqmQh6C9i6JavxzpRZCJejMikD25DVo0BjRlMQsg8iE6saQC5A5APINojk6kqyQNVWuW/MpRDCU7F2ECJ9y4AzEMlv5GjDeQjnJLF2GsBMeihS/GeQ+gvkVAT8BpHtyKRE+Ztf5KwkznHzCI03I8MliLjMC54QkQGQOaoeTJ0mF0GOUmROgP2xzvS+tQZlbaeIPF6uvY8MK1XrCRwWoAdkS2ziyD6Q/YZsgjF9xvvzwdrYATkCYyaE3TTmeDrr2foYGZarZkTAJu8rS4ArAHMRMLKRaFx0PfYh8EpEyiRLAhxaYSR5R+4BR4xzz1PJGcNrIPNF1dYIqN53WOduAjgAYAQhLMQZXw0weN+dTkqSdXz7aoAgp7zqBWvtLqdaUWtfpJIbGb4vet+RAt4F2q21bwDMI5ucaNJsrbmjMdlZrmogyLhvhshWkF5V9/UC72uznFjbu+a4bdBXkW1JNUlnoz72nrUnDXA7zfo3i1wk0F9SnayGNzwBE0Cbs3YcIsfWxSSfeNWhPuBzfdyab0oZyBtjeijSCZGW5cb9KuTb+DMpVef/j6y/AJR/RahTAGaeAAAAAElFTkSuQmCC" class="hot-img">
                    <span>热门标签</span>
                </div>
                <div class="temp-loading" v-if="request_of"></div><!--加载中-->
                <div class="web-empty-data" v-if="!request_of && literature_recommend_sys_temp2_allList && literature_recommend_sys_temp2_allList.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
                </div><!--暂无数据-->
                <div class="box-label" v-for="(it,i) in literature_recommend_sys_temp2_allList">
                    <span class="b-l-title">{{it.name}}</span>
                    <div class="b-l-list">
                        <span class="label-name" v-for="ite in it.subjectNames" @click="literature_recommend_sys_temp2_openurl(ite.detailUrl)">{{ite.word}}</span>
                        <!--<span class="more">更多></span>-->
                    </div>
                </div>
            </div><!--right end-->

        </div>
    </div>`;

    var list = document.getElementsByClassName('literature_recommend_sys_temp2');
    for (var i = 0; i < list.length; i++) {
        if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
            list[i].setAttribute('class', 'literature_recommend_sys_temp2 jl_vip_zt_vray jl_vip_zt_warp');
            var literature_recommend_sys_temp2_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
            new Vue({
                el: '#' + list[i].lastChild.id,
                template: literature_recommend_sys_temp2_html,
                data() {
                    return {
                        request_of:true,//请求中
                        literature_recommend_sys_temp2_allList_fist: [],//第一个数组
                        literature_recommend_sys_temp2_allList: [],//标签列表
                        fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
                        post_url_vip: window.apiDomainAndPort,
                        default_img: 'this.src="'+window.localStorage.getItem('fileUrl')+'/public/image/img-error-174x241.jpg"',
                    }
                },
                mounted() {
                    if (literature_recommend_sys_temp2_set_list && literature_recommend_sys_temp2_set_list.length > 0) {
                        var list = [];
                        for (var i = 0; i < literature_recommend_sys_temp2_set_list.length; i++) {
                            var set_content = literature_recommend_sys_temp2_set_list[i];
                            var topCount = '';
                            var columnid = '';
                            var OrderRule = '';
                            if (set_content) {
                                topCount = set_content['topCount'];
                                columnid = set_content['id'];
                                OrderRule = set_content['sortType'];
                            }
                            list.push({ Count: topCount, ColumnId: columnid, OrderRule: OrderRule });
                        }
                        this.literature_recommend_sys_temp2_initData(list);
                    }
                },
                methods: {
                    literature_recommend_sys_temp2_openurl(url) {
                        window.open(url)
                    },
                    literature_recommend_sys_temp2_initData(list) {
                        var post_url = this.post_url_vip + '/articlerecommend/api/sceneuse/getsceneusebyidbatch';
                        axios({
                            url: post_url,
                            method: 'post',
                            data: list,
                            headers: {
                                'Content-Type': 'application/json',
                                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
                            },
                        }).then(res => {
                            this.request_of = false;
                            if (res.data && res.data.statusCode == 200) {
                                if (res.data.data.length > 0) {
                                    this.literature_recommend_sys_temp2_allList_fist = (res.data.data[0].titleInfos || []);
                                    for (var i = 0; i < res.data.data.length; i++) {
                                        // if(i==0){
                                        //     continue;
                                        // }
                                        this.literature_recommend_sys_temp2_allList.push((res.data.data[i] || []));
                                    }
                                }
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
literature_recommend_sys_temp2()