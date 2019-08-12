<template>
  <section class="lemma">
    <span>{{lemmaWithStressMarker}}</span>
  </section>
</template>

<script>
export default {
  props: {
    lemma: String,
    stressIndex: Number
  },
  computed: {
    lemmaWithStressMarker: function() {
      return (
        this.lemma &&
        this.combineLemmaWithStressIndicator(this.lemma, this.stressIndex)
      );
    }
  },
  methods: {
    combineLemmaWithStressIndicator(string, stressIndex) {
      if (string == null) {
        return "";
      }
      if (stressIndex == null) {
        return string;
      }
      return (
        string.substring(0, stressIndex) +
        String.fromCharCode(769) +
        string.substring(stressIndex)
      );
    }
  }
};
</script>

<style lang="scss">
.lemma {
  font-weight: bold;
}
</style>